"use client"

import { useState } from "react"
import { Wrench, Plus, Pencil, Trash2, Search } from "lucide-react"
import { ServiceFormModal } from "./service-form-modal"
import { useCreateService, useUpdateService } from "../hooks/use-service"
import { Input } from "@/components/ui/input"
import { Badge } from "@/components/ui/badge"
import { TableSkeleton } from "@/components/shared/table-skeleton"
import type { Service, CreateServiceDto } from "../types/service.types"

const currencyFormatter = new Intl.NumberFormat("es-CO", {
  style: "currency",
  currency: "COP",
  maximumFractionDigits: 0,
})

interface ServiceTableProps {
  services: Service[]
  isLoading: boolean
  search: string
  onSearchChange: (search: string) => void
  onDelete: (id: number) => void
}

export function ServiceTable({ services, isLoading, search, onSearchChange, onDelete }: ServiceTableProps) {
  const [isModalOpen, setIsModalOpen]       = useState(false)
  const [editingService, setEditingService] = useState<Service | undefined>(undefined)

  const createService = useCreateService()
  const updateService = useUpdateService()
  const isSaving       = createService.isPending || updateService.isPending

  function handleEdit(service: Service) {
    setEditingService(service)
    setIsModalOpen(true)
  }

  function handleOpenChange(open: boolean) {
    setIsModalOpen(open)
    if (!open) setEditingService(undefined)
  }

  function handleSave(dto: CreateServiceDto, id?: number) {
    if (id !== undefined) {
      updateService.mutate(
        { ...dto, id },
        { onSuccess: () => handleOpenChange(false) }
      )
    } else {
      createService.mutate(dto, {
        onSuccess: () => handleOpenChange(false),
      })
    }
  }

  if (isLoading) {
    return (
      <TableSkeleton
        title="Lista de Servicios"
        columns={5}
        columnWidths={["w-32", "w-40", "w-20", "w-20", "w-24"]}
      />
    )
  }

  return (
    <>
      <div className="rounded-lg border border-border bg-card p-6 shadow-md">

        {/* Header */}
        <div className="flex items-center justify-between mb-4">
          <div className="flex items-center gap-2">
            <Wrench className="h-5 w-5 text-primary" />
            <h2 className="text-xl font-bold text-foreground">Lista de Servicios</h2>
          </div>
          <button
            onClick={() => { setEditingService(undefined); setIsModalOpen(true) }}
            className="flex items-center gap-2 bg-primary text-primary-foreground px-4 py-2 rounded-lg text-sm hover:opacity-90 transition"
          >
            <Plus className="h-4 w-4" />
            Nuevo servicio
          </button>
        </div>

        {/* Buscador */}
        <div className="relative mb-4 max-w-xs">
          <Search className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
          <Input
            placeholder="Buscar servicio..."
            value={search}
            onChange={(e) => onSearchChange(e.target.value)}
            className="pl-9"
          />
        </div>

        {/* Tabla */}
        <table className="w-full text-left">
          <thead>
            <tr className="bg-muted">
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Nombre</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Descripción</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Precio</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Estado</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Acciones</th>
            </tr>
          </thead>
          <tbody>

            {/* Empty */}
            {services.length === 0 && (
              <tr>
                <td colSpan={5} className="py-16 text-center text-sm text-muted-foreground">
                  <Wrench className="mx-auto mb-2 size-8 opacity-25" />
                  No hay servicios registrados
                </td>
              </tr>
            )}

            {/* Filas */}
            {services.map((service) => (
              <tr
                key={service.id}
                className="border-b border-border hover:bg-accent transition-colors"
              >
                <td className="py-4 px-4 text-foreground font-medium">{service.name}</td>
                <td className="py-4 px-4 text-muted-foreground">{service.description || "—"}</td>
                <td className="py-4 px-4 text-muted-foreground font-mono">{currencyFormatter.format(service.price)}</td>

                {/* Badge Estado */}
                <td className="py-4 px-4">
                  {service.isActive ? (
                    <Badge className="bg-emerald-100 text-emerald-800 hover:bg-emerald-100">Activo</Badge>
                  ) : (
                    <Badge className="bg-red-100 text-red-700 hover:bg-red-100">Inactivo</Badge>
                  )}
                </td>

                {/* Acciones */}
                <td className="py-4 px-4">
                  <div className="flex items-center gap-2">
                    <button
                      onClick={() => handleEdit(service)}
                      className="bg-secondary text-secondary-foreground text-xs font-semibold px-3 py-1 rounded-full hover:opacity-90 transition"
                    >
                      <Pencil className="h-3 w-3 inline mr-1" />
                      Editar
                    </button>
                    <button
                      onClick={() => {
                        if (window.confirm(`¿Eliminar el servicio "${service.name}"? Esta acción no se puede deshacer.`)) {
                          onDelete(service.id)
                        }
                      }}
                      className="bg-destructive/10 text-destructive text-xs font-semibold px-3 py-1 rounded-full hover:bg-destructive/20 transition"
                    >
                      <Trash2 className="h-3 w-3 inline mr-1" />
                      Eliminar
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Modal */}
      <ServiceFormModal
        open={isModalOpen}
        onOpenChange={handleOpenChange}
        isSaving={isSaving}
        onSave={handleSave}
        service={editingService}
      />
    </>
  )
}
