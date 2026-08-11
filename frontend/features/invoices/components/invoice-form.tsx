"use client"

import { useState } from "react"
import { useRouter } from "next/navigation"
import { Plus, Trash2, Receipt } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { useAllOwners } from "@/features/owners/hooks/use-owners"
import { useBillableAppointments } from "@/features/appointments/hooks/use-appointments"
import { useSupplies } from "@/features/supply/hook/use-supplies"
import { useServices } from "@/features/services/hooks/use-service"
import { useCreateInvoice } from "../hooks/use-invoices"
import { InvoiceItemType } from "../types/invoice.types"

interface ItemRow {
  key: string
  itemType: InvoiceItemType
  description: string
  supplyId: number | null
  serviceId: number | null
  quantity: number
  unitPrice: number
}

function emptyRow(): ItemRow {
  return {
    key: crypto.randomUUID(),
    itemType: InvoiceItemType.Service,
    description: "",
    supplyId: null,
    serviceId: null,
    quantity: 1,
    unitPrice: 0,
  }
}

export function InvoiceForm() {
  const router = useRouter()

  const [ownerId, setOwnerId] = useState("")
  const [appointmentId, setAppointmentId] = useState("")
  const [items, setItems] = useState<ItemRow[]>([emptyRow()])

  const { data: ownersData } = useAllOwners()
  const { data: appointments = [] } = useBillableAppointments(Number(ownerId))
  const { data: suppliesData } = useSupplies(1, 100, "", undefined, true)
  const { data: servicesData } = useServices(1, 100, "")

  const owners = ownersData?.items ?? []
  const supplies = suppliesData?.items ?? []
  const services = servicesData ?? []

  const createInvoice = useCreateInvoice()

  const subtotal = items.reduce((acc, i) => acc + i.quantity * i.unitPrice, 0)

  const addItem = () => setItems((prev) => [...prev, emptyRow()])
  const removeItem = (key: string) => setItems((prev) => prev.filter((i) => i.key !== key))

  const updateItem = (key: string, patch: Partial<ItemRow>) => {
    setItems((prev) => prev.map((i) => (i.key === key ? { ...i, ...patch } : i)))
  }

  const handleTypeChange = (key: string, itemType: InvoiceItemType) => {
    updateItem(key, { itemType, description: "", supplyId: null, serviceId: null, unitPrice: 0 })
  }

  const handleSupplyChange = (key: string, supplyId: string) => {
    const supply = supplies.find((s) => s.id === Number(supplyId))
    updateItem(key, { supplyId: Number(supplyId), description: supply?.name ?? "" })
  }

  const handleServiceChange = (key: string, serviceId: string) => {
    const service = services.find((s) => s.id === Number(serviceId))
    updateItem(key, {
      serviceId: Number(serviceId),
      description: service?.name ?? "",
      unitPrice: service?.price ?? 0,
    })
  }

  const isValid =
    !!ownerId &&
    items.length > 0 &&
    items.every((i) => {
      if (i.quantity <= 0) return false
      if (i.itemType === InvoiceItemType.Service && !i.serviceId) return false
      if (i.itemType === InvoiceItemType.Supply) {
        if (!i.supplyId) return false
        const supply = supplies.find((s) => s.id === i.supplyId)
        if (supply && i.quantity > supply.currentStock) return false
      }
      return true
    })

  const handleSubmit = () => {
    if (!isValid) return

    createInvoice.mutate(
      {
        ownerId: Number(ownerId),
        appointmentId: appointmentId ? Number(appointmentId) : null,
        items: items.map((i) => ({
          itemType: i.itemType,
          description: i.description,
          supplyId: i.itemType === InvoiceItemType.Supply ? i.supplyId : null,
          quantity: i.quantity,
          unitPrice: i.unitPrice,
        })),
      },
      {
        onSuccess: (invoice) => {
          router.push(`/invoices/${invoice.id}`)
        },
      }
    )
  }

  return (
    <div className="rounded-2xl border border-gray-200 bg-white p-6 shadow-md space-y-6">
      <div className="flex items-center gap-2">
        <Receipt className="h-5 w-5 text-primary" />
        <h2 className="text-xl font-bold text-gray-800">Nueva factura</h2>
      </div>

      {/* Owner / Appointment */}
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
        <div className="flex flex-col gap-1.5">
          <Label>Propietario</Label>
          <Select
            value={ownerId}
            onValueChange={(v) => {
              setOwnerId(v)
              setAppointmentId("")
            }}
          >
            <SelectTrigger className="w-full">
              <SelectValue placeholder="Selecciona un propietario" />
            </SelectTrigger>
            <SelectContent>
              {owners.map((o) => (
                <SelectItem key={o.id} value={String(o.id)}>
                  {o.ownerName}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="flex flex-col gap-1.5">
          <Label>Cita asociada (opcional)</Label>
          <Select value={appointmentId} onValueChange={setAppointmentId} disabled={!ownerId}>
            <SelectTrigger className="w-full">
              <SelectValue placeholder="Sin cita asociada" />
            </SelectTrigger>
            <SelectContent>
              {appointments.map((a) => (
                <SelectItem key={a.id} value={String(a.id)}>
                  {new Date(a.appointmentDate).toLocaleDateString("es-CO")} — {a.petName}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
      </div>

      {/* Items */}
      <div className="space-y-3">
        <div className="flex items-center justify-between">
          <h3 className="font-semibold text-gray-800">Ítems</h3>
          <button
            onClick={addItem}
            className="flex items-center gap-1 bg-blue-100 text-blue-700 text-xs font-semibold px-3 py-1.5 rounded-full hover:bg-blue-200 transition"
          >
            <Plus className="h-3 w-3" />
            Agregar ítem
          </button>
        </div>

        <div className="space-y-3">
          {items.map((item) => {
            const supply = supplies.find((s) => s.id === item.supplyId)
            const overStock =
              item.itemType === InvoiceItemType.Supply && !!supply && item.quantity > supply.currentStock

            return (
              <div key={item.key} className="rounded-xl border border-gray-200 p-4 space-y-3">
                <div className="grid grid-cols-1 sm:grid-cols-[140px_1fr] gap-3">
                  <div className="flex flex-col gap-1.5">
                    <Label className="text-xs text-gray-500">Tipo</Label>
                    <Select
                      value={String(item.itemType)}
                      onValueChange={(v) => handleTypeChange(item.key, Number(v) as InvoiceItemType)}
                    >
                      <SelectTrigger className="w-full">
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value={String(InvoiceItemType.Service)}>Servicio</SelectItem>
                        <SelectItem value={String(InvoiceItemType.Supply)}>Insumo</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>

                  {item.itemType === InvoiceItemType.Service ? (
                    <div className="flex flex-col gap-1.5">
                      <Label className="text-xs text-gray-500">Servicio</Label>
                      <Select
                        value={item.serviceId ? String(item.serviceId) : ""}
                        onValueChange={(v) => handleServiceChange(item.key, v)}
                      >
                        <SelectTrigger className="w-full">
                          <SelectValue placeholder="Selecciona un servicio" />
                        </SelectTrigger>
                        <SelectContent>
                          {services.map((s) => (
                            <SelectItem key={s.id} value={String(s.id)}>
                              {s.name} (${s.price.toLocaleString("es-CO", { minimumFractionDigits: 2 })})
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </div>
                  ) : (
                    <div className="flex flex-col gap-1.5">
                      <Label className="text-xs text-gray-500">Insumo</Label>
                      <Select
                        value={item.supplyId ? String(item.supplyId) : ""}
                        onValueChange={(v) => handleSupplyChange(item.key, v)}
                      >
                        <SelectTrigger className="w-full">
                          <SelectValue placeholder="Selecciona un insumo" />
                        </SelectTrigger>
                        <SelectContent>
                          {supplies.map((s) => (
                            <SelectItem key={s.id} value={String(s.id)}>
                              {s.name} (stock: {s.currentStock})
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </div>
                  )}
                </div>

                <div className="grid grid-cols-2 sm:grid-cols-4 gap-3 items-end">
                  <div className="flex flex-col gap-1.5">
                    <Label className="text-xs text-gray-500">Cantidad</Label>
                    <Input
                      type="number"
                      min={1}
                      value={item.quantity}
                      onChange={(e) => updateItem(item.key, { quantity: Number(e.target.value) })}
                    />
                    {supply && (
                      <span className={`text-xs ${overStock ? "text-red-600 font-semibold" : "text-gray-400"}`}>
                        Disponible: {supply.currentStock}
                      </span>
                    )}
                  </div>

                  <div className="flex flex-col gap-1.5">
                    <Label className="text-xs text-gray-500">Precio unitario</Label>
                    <Input
                      type="number"
                      min={0}
                      step="0.01"
                      value={item.unitPrice}
                      onChange={(e) => updateItem(item.key, { unitPrice: Number(e.target.value) })}
                    />
                  </div>

                  <div className="flex flex-col gap-1.5">
                    <Label className="text-xs text-gray-500">Subtotal ítem</Label>
                    <div className="h-9 flex items-center text-sm font-medium text-gray-700">
                      ${(item.quantity * item.unitPrice).toLocaleString("es-CO", { minimumFractionDigits: 2 })}
                    </div>
                  </div>

                  <div className="flex justify-end">
                    <button
                      onClick={() => removeItem(item.key)}
                      disabled={items.length === 1}
                      className="flex items-center gap-1 bg-red-100 text-red-700 text-xs font-semibold px-3 py-1.5 rounded-full hover:bg-red-200 transition disabled:opacity-40"
                    >
                      <Trash2 className="h-3 w-3" />
                      Quitar
                    </button>
                  </div>
                </div>
              </div>
            )
          })}
        </div>
      </div>

      {/* Totales */}
      <div className="rounded-xl bg-gray-50 border border-gray-200 p-4 space-y-1">
        <p className="text-xs text-gray-400 mb-2">
          Previsualización — el total final se calcula y confirma al guardar en el servidor.
        </p>
        <div className="flex justify-between text-sm text-gray-600">
          <span>Subtotal</span>
          <span>${subtotal.toLocaleString("es-CO", { minimumFractionDigits: 2 })}</span>
        </div>
        <div className="flex justify-between text-sm text-gray-600">
          <span>IVA</span>
          <span>Se calcula al guardar</span>
        </div>
        <div className="flex justify-between text-base font-bold text-gray-800 pt-1 border-t border-gray-200 mt-1">
          <span>Total</span>
          <span>Se calcula al guardar</span>
        </div>
      </div>

      {/* Acciones */}
      <div className="flex justify-end gap-3">
        <Button type="button" variant="outline" onClick={() => router.push("/invoices")}>
          Cancelar
        </Button>
        <Button
          type="button"
          onClick={handleSubmit}
          disabled={!isValid || createInvoice.isPending}
          className="bg-[#1F6F5C] text-white hover:bg-[#18594a]"
        >
          {createInvoice.isPending ? "Guardando..." : "Guardar factura"}
        </Button>
      </div>
    </div>
  )
}