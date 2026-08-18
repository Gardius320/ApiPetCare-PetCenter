"use client"

import { useState } from "react"
import { Package, Plus, Pencil, Trash2 } from "lucide-react"
import { SupplyFormModal } from "./supply-form-modal"
import { SupplyCategoriesModal } from "../../supply-categories/components/supply-categories-modal"
import { useCreateSupply, useUpdateSupply, useToggleSupplyStatus } from "../hook/use-supplies"
import type { Supply, CreateSupplyDto, UpdateSupplyDto } from "../types/supply.types"
import { TableSkeleton } from "@/components/shared/table-skeleton"

interface SuppliesTableProps {
  supplies: Supply[]
  totalRecords: number
  page: number
  pageSize: number
  isLoading: boolean
  onPageChange: (page: number) => void
}

export function SuppliesTable({
  supplies,
  totalRecords,
  page,
  pageSize,
  isLoading,
  onPageChange,
}: SuppliesTableProps) {
  const [isModalOpen, setIsModalOpen]     = useState(false)
  const [editingSupply, setEditingSupply] = useState<Supply | undefined>(undefined)
  const [isCategoriesModalOpen, setIsCategoriesModalOpen] = useState(false)

  const createSupply = useCreateSupply()
  const updateSupply = useUpdateSupply()
  const toggleStatus = useToggleSupplyStatus()
  const isSaving      = createSupply.isPending || updateSupply.isPending

  const totalPages = Math.max(1, Math.ceil(totalRecords / pageSize))

  function handleEdit(supply: Supply) {
    setEditingSupply(supply)
    setIsModalOpen(true)
  }

  function handleOpenChange(open: boolean) {
    setIsModalOpen(open)
    if (!open) setEditingSupply(undefined)
  }

  function handleSave(dto: CreateSupplyDto | UpdateSupplyDto, id?: number) {
    if (id !== undefined) {
      updateSupply.mutate(
        { id, dto: dto as UpdateSupplyDto },
        { onSuccess: () => handleOpenChange(false) }
      )
    } else {
      createSupply.mutate(dto as CreateSupplyDto, {
        onSuccess: () => handleOpenChange(false),
      })
    }
  }

  if (isLoading) {
    return (
      <TableSkeleton
        title="Lista de Insumos"
        columns={5}
        columnWidths={["w-32", "w-24", "w-20", "w-24", "w-16"]}
      />
    )
  }

  return (
    <>
      <div className="rounded-lg border border-border bg-card p-6 shadow-md">

        {/* Header */}
        <div className="flex items-center justify-between mb-4">
          <div className="flex items-center gap-2">
            <Package className="h-5 w-5 text-primary" />
            <h2 className="text-xl font-bold text-foreground">Lista de Insumos</h2>
          </div>
          <div className="flex gap-2">
            <button
              onClick={() => setIsCategoriesModalOpen(true)}
              className="flex items-center gap-2 bg-muted text-foreground px-4 py-2 rounded-lg text-sm hover:bg-accent transition"
            >
              Gestionar categorías
            </button>
            <button
              onClick={() => { setEditingSupply(undefined); setIsModalOpen(true) }}
              className="flex items-center gap-2 bg-primary text-primary-foreground px-4 py-2 rounded-lg text-sm hover:opacity-90 transition"
            >
              <Plus className="h-4 w-4" />
              Nuevo insumo
            </button>
          </div>
        </div>

        {/* Tabla */}
        <table className="w-full text-left">
          <thead>
            <tr className="bg-muted">
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Nombre</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Categoría</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Unidad</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Stock</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Acciones</th>
            </tr>
          </thead>
          <tbody>

            {/* Empty */}
            {supplies.length === 0 && (
              <tr>
                <td colSpan={5} className="py-16 text-center text-sm text-muted-foreground">
                  <Package className="mx-auto mb-2 size-8 opacity-25" />
                  No se encontraron insumos
                </td>
              </tr>
            )}

            {/* Filas */}
            {supplies.map((supply) => {
              const lowStock = supply.currentStock <= supply.minimumStock

              return (
                <tr
                  key={supply.id}
                  className="border-b border-border hover:bg-accent transition-colors"
                >
                  <td className="py-4 px-4 text-foreground font-medium">{supply.name}</td>
                  <td className="py-4 px-4 text-muted-foreground">{supply.categoryName}</td>
                  <td className="py-4 px-4 text-muted-foreground">{supply.unit}</td>
                  <td className="py-4 px-4">
                    <span className={lowStock ? "text-orange-600 font-semibold" : "text-muted-foreground"}>
                      {supply.currentStock} / {supply.minimumStock}
                    </span>
                  </td>

                  {/* Acciones */}
                  <td className="py-4 px-4">
                    <div className="flex items-center gap-2">
                      <button
                        onClick={() => handleEdit(supply)}
                        className="bg-secondary text-secondary-foreground text-xs font-semibold px-3 py-1 rounded-full hover:opacity-90 transition"
                      >
                        <Pencil className="h-3 w-3 inline mr-1" />
                        Editar
                      </button>
                      <button
                        onClick={() => {
                          if (window.confirm(`¿Eliminar "${supply.name}"? Podrás recuperarlo después si lo necesitas.`)) {
                            toggleStatus.mutate(supply.id)
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
              )
            })}
          </tbody>
        </table>

        {/* Paginación */}
        {totalPages > 1 && (
          <div className="flex items-center justify-between border-t border-border px-2 py-3 mt-2 text-sm text-muted-foreground">
            <span>
              Mostrando {(page - 1) * pageSize + 1}–{Math.min(page * pageSize, totalRecords)} de {totalRecords} insumos
            </span>
            <div className="flex gap-1">
              <button
                disabled={page === 1}
                onClick={() => onPageChange(page - 1)}
                className="px-3 py-1 rounded border border-border hover:bg-muted disabled:opacity-40 transition"
              >
                ←
              </button>

              {Array.from({ length: totalPages }, (_, i) => i + 1).map((p) => (
                <button
                  key={p}
                  onClick={() => onPageChange(p)}
                  className={`px-3 py-1 rounded border transition ${
                    p === page
                      ? "bg-primary text-primary-foreground border-primary"
                      : "border-border hover:bg-muted"
                  }`}
                >
                  {p}
                </button>
              ))}

              <button
                disabled={page === totalPages}
                onClick={() => onPageChange(page + 1)}
                className="px-3 py-1 rounded border border-border hover:bg-muted disabled:opacity-40 transition"
              >
                →
              </button>
            </div>
          </div>
        )}
      </div>

      {/* Modal de insumo */}
      <SupplyFormModal
        open={isModalOpen}
        onOpenChange={handleOpenChange}
        isSaving={isSaving}
        onSave={handleSave}
        supply={editingSupply}
      />

      {/* Modal de categorías */}
      <SupplyCategoriesModal
        open={isCategoriesModalOpen}
        onOpenChange={setIsCategoriesModalOpen}
      />
    </>
  )
}