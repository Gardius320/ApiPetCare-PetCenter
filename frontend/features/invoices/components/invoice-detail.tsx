"use client"

import { useEffect, useState } from "react"
import { Printer, Ban, Receipt, CreditCard } from "lucide-react"
import { Button } from "@/components/ui/button"
import { useInvoice, useCancelInvoice } from "../hooks/use-invoices"
import { useAllOwners } from "@/features/owners/hooks/use-owners"
import { useAppointments } from "@/features/appointments/hooks/use-appointments"
import { InvoiceStatusBadge } from "./invoice-status-badge"
import { InvoiceItemType, InvoiceStatus, PAYMENT_METHOD_LABELS } from "../types/invoice.types"

interface Props {
  id: string
}

export function InvoiceDetail({ id }: Props) {
  const { data, isLoading } = useInvoice(id)
  const { data: ownersData } = useAllOwners()
  const { data: appointmentsData } = useAppointments(1, 100, "")
  const cancelInvoice = useCancelInvoice()

 const [role] = useState<string | null>(() =>
  typeof window !== "undefined" ? localStorage.getItem("role") : null
)

  if (isLoading || !data) {
    return <p className="p-4 text-sm text-gray-500">Cargando factura...</p>
  }

  const { invoice, items } = data
  const owner = ownersData?.items.find((o) => o.id === invoice.ownerId)
  const appointment = appointmentsData?.items.find((a) => a.id === invoice.appointmentId)

  const canCancel = role === "Admin" && invoice.status !== InvoiceStatus.Cancelled

  const handleCancel = () => {
    if (!window.confirm(`¿Anular la factura ${invoice.invoiceNumber}? Esta acción no se puede deshacer.`)) return
    cancelInvoice.mutate(id)
  }

  return (
    <div className="space-y-4">
      {/* Acciones */}
      <div className="no-print flex items-center justify-between">
        <h1 className="text-2xl font-bold text-foreground">Factura {invoice.invoiceNumber}</h1>
        <div className="flex gap-2">
          <Button variant="outline" onClick={() => window.print()}>
            <Printer className="h-4 w-4 mr-1" />
            Imprimir
          </Button>
          {canCancel && (
            <Button variant="destructive" onClick={handleCancel} disabled={cancelInvoice.isPending}>
              <Ban className="h-4 w-4 mr-1" />
              {cancelInvoice.isPending ? "Anulando..." : "Anular factura"}
            </Button>
          )}
        </div>
      </div>

      {/* Contenido imprimible */}
      <div className="invoice-detail rounded-2xl border border-gray-200 bg-white p-6 shadow-md space-y-6">
        <div className="flex items-start justify-between">
          <div className="flex items-center gap-2">
            <Receipt className="h-5 w-5 text-primary" />
            <div>
              <p className="text-lg font-bold text-gray-800">{invoice.invoiceNumber}</p>
              <p className="text-sm text-gray-500">
                {new Date(invoice.issueDate).toLocaleDateString("es-CO")}
              </p>
            </div>
          </div>
          <InvoiceStatusBadge status={invoice.status} />
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 text-sm">
          <div>
            <p className="text-gray-400">Propietario</p>
            <p className="font-medium text-gray-800">{owner?.ownerName ?? `#${invoice.ownerId}`}</p>
          </div>
          <div>
            <p className="text-gray-400">Cita asociada</p>
            <p className="font-medium text-gray-800">
              {invoice.appointmentId
                ? appointment
                  ? `${new Date(appointment.date).toLocaleDateString("es-CO")} — ${appointment.petName}`
                  : `Cita #${invoice.appointmentId}`
                : "Sin cita asociada"}
            </p>
          </div>
        </div>

        {/* Items */}
        <table className="w-full text-left">
          <thead>
            <tr className="bg-gray-50">
              <th className="border-b border-gray-200 py-2 px-3 text-sm text-gray-600">Descripción</th>
              <th className="border-b border-gray-200 py-2 px-3 text-sm text-gray-600">Tipo</th>
              <th className="border-b border-gray-200 py-2 px-3 text-sm text-gray-600 text-right">Cantidad</th>
              <th className="border-b border-gray-200 py-2 px-3 text-sm text-gray-600 text-right">Precio unitario</th>
              <th className="border-b border-gray-200 py-2 px-3 text-sm text-gray-600 text-right">Subtotal</th>
            </tr>
          </thead>
          <tbody>
            {items.map((item) => (
              <tr key={item.id} className="border-b border-gray-100">
                <td className="py-3 px-3 text-gray-700">{item.description}</td>
                <td className="py-3 px-3 text-gray-500">
                  {item.itemType === InvoiceItemType.Service ? "Servicio" : "Insumo"}
                </td>
                <td className="py-3 px-3 text-right text-gray-600">{item.quantity}</td>
                <td className="py-3 px-3 text-right text-gray-600">
                  ${item.unitPrice.toLocaleString("es-CO", { minimumFractionDigits: 2 })}
                </td>
                <td className="py-3 px-3 text-right text-gray-700 font-medium">
                  ${item.lineTotal.toLocaleString("es-CO", { minimumFractionDigits: 2 })}
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {/* Totales */}
        <div className="flex justify-end">
          <div className="w-full sm:w-64 space-y-1">
            <div className="flex justify-between text-sm text-gray-600">
              <span>Subtotal</span>
              <span>${invoice.subtotal.toLocaleString("es-CO", { minimumFractionDigits: 2 })}</span>
            </div>
            <div className="flex justify-between text-sm text-gray-600">
              <span>IVA</span>
              <span>${invoice.tax.toLocaleString("es-CO", { minimumFractionDigits: 2 })}</span>
            </div>
            <div className="flex justify-between text-base font-bold text-gray-800 pt-1 border-t border-gray-200 mt-1">
              <span>Total</span>
              <span>${invoice.total.toLocaleString("es-CO", { minimumFractionDigits: 2 })}</span>
            </div>
          </div>
        </div>

        {/* Información de pago (solo si ya está pagada) */}
        {invoice.status === InvoiceStatus.Paid && (
          <div className="rounded-xl bg-green-50 border border-green-200 p-4">
            <div className="flex items-center gap-2 mb-3">
              <CreditCard className="h-4 w-4 text-green-700" />
              <h3 className="text-sm font-semibold text-green-800">Información de pago</h3>
            </div>
            <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 text-sm">
              <div>
                <p className="text-gray-400">Método</p>
                <p className="font-medium text-gray-800">
                  {invoice.paymentMethod !== null ? PAYMENT_METHOD_LABELS[invoice.paymentMethod] : "—"}
                </p>
              </div>
              <div>
                <p className="text-gray-400">Referencia</p>
                <p className="font-medium text-gray-800">{invoice.paymentReference ?? "—"}</p>
              </div>
              <div>
                <p className="text-gray-400">Fecha de pago</p>
                <p className="font-medium text-gray-800">
                  {invoice.paymentDate
                    ? new Date(invoice.paymentDate).toLocaleDateString("es-CO")
                    : "—"}
                </p>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  )
}