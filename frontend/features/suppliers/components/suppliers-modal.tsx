"use client"

import { Fragment, useState } from "react"
import { Truck, Plus, Pencil, Trash2, Eye } from "lucide-react"
import { Dialog, DialogContent } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { FormDialogHeader } from "@/components/shared/form-dialog-header"
import {
  useAllSuppliers, useCreateSupplier,
  useUpdateSupplier, useDeleteSupplier,
} from "../hooks/use-suppliers"
import { SupplierFormModal } from "./supplier-form-modal"
import type { Supplier, CreateSupplierDto } from "../types/supplier.types"

interface SuppliersModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
}

export function SuppliersModal({ open, onOpenChange }: SuppliersModalProps) {
  const [isFormOpen, setIsFormOpen] = useState(false)
  const [editingSupplier, setEditingSupplier] = useState<Supplier | undefined>(undefined)
  const [expandedId, setExpandedId] = useState<number | null>(null)

  const { data: suppliers, isLoading } = useAllSuppliers({ onlyActive: false, pageSize: 100 })
  const createSupplier = useCreateSupplier()
  const updateSupplier = useUpdateSupplier()
  const deleteSupplier = useDeleteSupplier()
  const isSaving = createSupplier.isPending || updateSupplier.isPending

  function handleEdit(supplier: Supplier) {
    setEditingSupplier(supplier)
    setIsFormOpen(true)
  }

  function handleFormOpenChange(nextOpen: boolean) {
    setIsFormOpen(nextOpen)
    if (!nextOpen) setEditingSupplier(undefined)
  }

  function handleSave(dto: CreateSupplierDto, id?: number) {
    if (id !== undefined) {
      updateSupplier.mutate({ id, dto }, { onSuccess: () => handleFormOpenChange(false) })
    } else {
      createSupplier.mutate(dto, { onSuccess: () => handleFormOpenChange(false) })
    }
  }

  function handleDelete(supplier: Supplier) {
    if (window.confirm(`¿Eliminar el proveedor "${supplier.name}"? Esta acción no se puede deshacer.`)) {
      deleteSupplier.mutate(supplier.id)
    }
  }

  return (
    <>
      <Dialog open={open} onOpenChange={onOpenChange}>
        <DialogContent className="sm:max-w-2xl">
          <FormDialogHeader icon={Truck} title="Proveedores" accent="#FF8C6B" />

          <div className="flex justify-end -mt-2 mb-2">
            <Button
              onClick={() => { setEditingSupplier(undefined); setIsFormOpen(true) }}
              className="bg-primary text-primary-foreground hover:bg-primary/90"
              size="sm"
            >
              <Plus className="h-4 w-4 mr-1" />
              Nuevo proveedor
            </Button>
          </div>

          <div className="max-h-80 overflow-y-auto">
            <table className="w-full text-left text-sm">
              <thead>
                <tr className="bg-muted">
                  <th className="border-b border-border py-2 px-3 text-[11px] font-medium uppercase tracking-wide text-muted-foreground">Nombre</th>
                  <th className="border-b border-border py-2 px-3 text-[11px] font-medium uppercase tracking-wide text-muted-foreground">Contacto</th>
                  <th className="border-b border-border py-2 px-3 text-[11px] font-medium uppercase tracking-wide text-muted-foreground">Acciones</th>
                </tr>
              </thead>
              <tbody>
                {isLoading && (
                  <tr><td colSpan={3} className="py-8 text-center text-muted-foreground">Cargando proveedores...</td></tr>
                )}
                {!isLoading && (suppliers?.length ?? 0) === 0 && (
                  <tr><td colSpan={3} className="py-8 text-center text-muted-foreground">No hay proveedores registrados</td></tr>
                )}
                {suppliers?.map((supplier) => (
                  <Fragment key={supplier.id}>
                    <tr className="border-b border-border transition-colors hover:bg-accent">
                      <td className="py-2 px-3 font-medium text-foreground">{supplier.name}</td>
                      <td className="py-2 px-3 text-muted-foreground whitespace-nowrap">
                        {supplier.contactNumber || supplier.email || "—"}
                      </td>
                      <td className="py-2 px-3">
                        <div className="flex items-center gap-2">
                          <button
                            onClick={() => setExpandedId(expandedId === supplier.id ? null : supplier.id)}
                            aria-label="Ver información"
                            className="rounded-full bg-accent px-2 py-1 text-primary transition hover:opacity-90"
                          >
                            <Eye className="h-3.5 w-3.5" />
                          </button>
                          <button
                            onClick={() => handleEdit(supplier)}
                            className="rounded-full bg-secondary px-3 py-1 text-xs font-semibold text-secondary-foreground transition hover:opacity-90"
                          >
                            <Pencil className="h-3 w-3 inline mr-1" />
                            Editar
                          </button>
                          <button
                            onClick={() => handleDelete(supplier)}
                            className="rounded-full bg-destructive/10 px-3 py-1 text-xs font-semibold text-destructive transition hover:bg-destructive/20"
                          >
                            <Trash2 className="h-3 w-3 inline mr-1" />
                            Eliminar
                          </button>
                        </div>
                      </td>
                    </tr>
                    {expandedId === supplier.id && (
                      <tr className="border-b border-border bg-muted/50">
                        <td colSpan={3} className="px-3 py-3 text-xs text-muted-foreground">
                          <div className="grid grid-cols-1 gap-1.5 sm:grid-cols-2">
                            <p><span className="font-medium text-foreground">Teléfono:</span> {supplier.contactNumber || "—"}</p>
                            <p><span className="font-medium text-foreground">Email:</span> {supplier.email || "—"}</p>
                            <p><span className="font-medium text-foreground">Dirección:</span> {supplier.address || "—"}</p>
                            <p className="sm:col-span-2"><span className="font-medium text-foreground">Qué trae:</span> {supplier.description || "—"}</p>
                          </div>
                        </td>
                      </tr>
                    )}
                  </Fragment>
                ))}
              </tbody>
            </table>
          </div>
        </DialogContent>
      </Dialog>

      <SupplierFormModal
        open={isFormOpen}
        onOpenChange={handleFormOpenChange}
        isSaving={isSaving}
        onSave={handleSave}
        supplier={editingSupplier}
      />
    </>
  )
}
