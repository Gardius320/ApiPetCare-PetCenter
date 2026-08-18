"use client"

import { useState } from "react"
import { Layers, Plus, Pencil, Trash2 } from "lucide-react"
import { Dialog, DialogContent } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { FormDialogHeader } from "@/components/shared/form-dialog-header"
import {
  useAllSupplyCategories, useCreateSupplyCategory,
  useUpdateSupplyCategory, useDeleteSupplyCategory,
} from "../hooks/use-supply-categories"
import { SupplyCategoryFormModal } from "./supply-category-form-modal"
import type { SupplyCategory, CreateSupplyCategoryDto } from "../types/supply-category.types"

interface SupplyCategoriesModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
}

export function SupplyCategoriesModal({ open, onOpenChange }: SupplyCategoriesModalProps) {
  const [isFormOpen, setIsFormOpen] = useState(false)
  const [editingCategory, setEditingCategory] = useState<SupplyCategory | undefined>(undefined)

  const { data: categories, isLoading } = useAllSupplyCategories({ onlyActive: false, pageSize: 100 })
  const createCategory = useCreateSupplyCategory()
  const updateCategory = useUpdateSupplyCategory()
  const deleteCategory = useDeleteSupplyCategory()
  const isSaving = createCategory.isPending || updateCategory.isPending

  function handleEdit(category: SupplyCategory) {
    setEditingCategory(category)
    setIsFormOpen(true)
  }

  function handleFormOpenChange(nextOpen: boolean) {
    setIsFormOpen(nextOpen)
    if (!nextOpen) setEditingCategory(undefined)
  }

  function handleSave(dto: CreateSupplyCategoryDto, id?: number) {
    if (id !== undefined) {
      updateCategory.mutate({ id, dto }, { onSuccess: () => handleFormOpenChange(false) })
    } else {
      createCategory.mutate(dto, { onSuccess: () => handleFormOpenChange(false) })
    }
  }

  function handleDelete(category: SupplyCategory) {
    if (window.confirm(`¿Eliminar la categoría "${category.name}"? Esta acción no se puede deshacer.`)) {
      deleteCategory.mutate(category.id)
    }
  }

  return (
    <>
      <Dialog open={open} onOpenChange={onOpenChange}>
        <DialogContent className="sm:max-w-lg">
          <FormDialogHeader icon={Layers} title="Categorías de insumos" accent="#E0A458" />

          <div className="flex justify-end -mt-2 mb-2">
            <Button
              onClick={() => { setEditingCategory(undefined); setIsFormOpen(true) }}
              className="bg-primary text-primary-foreground hover:bg-primary/90"
              size="sm"
            >
              <Plus className="h-4 w-4 mr-1" />
              Nueva categoría
            </Button>
          </div>

          <div className="max-h-80 overflow-y-auto">
            <table className="w-full text-left text-sm">
              <thead>
                <tr className="bg-muted">
                  <th className="border-b border-border py-2 px-3 text-[11px] font-medium uppercase tracking-wide text-muted-foreground">Nombre</th>
                  <th className="border-b border-border py-2 px-3 text-[11px] font-medium uppercase tracking-wide text-muted-foreground">Descripción</th>
                  <th className="border-b border-border py-2 px-3 text-[11px] font-medium uppercase tracking-wide text-muted-foreground">Acciones</th>
                </tr>
              </thead>
              <tbody>
                {isLoading && (
                  <tr><td colSpan={3} className="py-8 text-center text-muted-foreground">Cargando categorías...</td></tr>
                )}
                {!isLoading && (categories?.length ?? 0) === 0 && (
                  <tr><td colSpan={3} className="py-8 text-center text-muted-foreground">No hay categorías registradas</td></tr>
                )}
                {categories?.map((cat) => (
                  <tr key={cat.id} className="border-b border-border transition-colors hover:bg-accent">
                    <td className="py-2 px-3 font-medium text-foreground">{cat.name}</td>
                    <td className="py-2 px-3 text-muted-foreground">{cat.description || "—"}</td>
                    <td className="py-2 px-3">
                      <div className="flex items-center gap-2">
                        <button
                          onClick={() => handleEdit(cat)}
                          className="rounded-full bg-secondary px-3 py-1 text-xs font-semibold text-secondary-foreground transition hover:opacity-90"
                        >
                          <Pencil className="h-3 w-3 inline mr-1" />
                          Editar
                        </button>
                        <button
                          onClick={() => handleDelete(cat)}
                          className="rounded-full bg-destructive/10 px-3 py-1 text-xs font-semibold text-destructive transition hover:bg-destructive/20"
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
        </DialogContent>
      </Dialog>

      <SupplyCategoryFormModal
        open={isFormOpen}
        onOpenChange={handleFormOpenChange}
        isSaving={isSaving}
        onSave={handleSave}
        category={editingCategory}
      />
    </>
  )
}