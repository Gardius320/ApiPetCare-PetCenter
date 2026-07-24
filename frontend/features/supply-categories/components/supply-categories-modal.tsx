"use client"

import { useState } from "react"
import { Layers, Plus, Pencil, Trash2 } from "lucide-react"
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
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

  const { data: categories, isLoading } = useAllSupplyCategories()
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
          <DialogHeader>
            <div className="flex items-center gap-2">
              <Layers className="h-5 w-5 text-primary" />
              <DialogTitle>Categorías de insumos</DialogTitle>
            </div>
          </DialogHeader>

          <div className="flex justify-end -mt-2 mb-2">
            <Button
              onClick={() => { setEditingCategory(undefined); setIsFormOpen(true) }}
              className="bg-blue-400 hover:bg-blue-500 text-white"
              size="sm"
            >
              <Plus className="h-4 w-4 mr-1" />
              Nueva categoría
            </Button>
          </div>

          <div className="max-h-80 overflow-y-auto">
            <table className="w-full text-left text-sm">
              <thead>
                <tr className="bg-gray-50">
                  <th className="border-b border-gray-200 py-2 px-3 text-gray-600">Nombre</th>
                  <th className="border-b border-gray-200 py-2 px-3 text-gray-600">Descripción</th>
                  <th className="border-b border-gray-200 py-2 px-3 text-gray-600">Acciones</th>
                </tr>
              </thead>
              <tbody>
                {isLoading && (
                  <tr><td colSpan={3} className="py-8 text-center text-gray-400">Cargando categorías...</td></tr>
                )}
                {!isLoading && (categories?.length ?? 0) === 0 && (
                  <tr><td colSpan={3} className="py-8 text-center text-gray-400">No hay categorías registradas</td></tr>
                )}
                {categories?.map((cat) => (
                  <tr key={cat.id} className="border-b border-gray-100 hover:bg-blue-50 transition-colors">
                    <td className="py-2 px-3 text-gray-700 font-medium">{cat.name}</td>
                    <td className="py-2 px-3 text-gray-500">{cat.description || "—"}</td>
                    <td className="py-2 px-3">
                      <div className="flex items-center gap-2">
                        <button
                          onClick={() => handleEdit(cat)}
                          className="bg-blue-100 text-blue-700 text-xs font-semibold px-3 py-1 rounded-full hover:bg-blue-200 transition"
                        >
                          <Pencil className="h-3 w-3 inline mr-1" />
                          Editar
                        </button>
                        <button
                          onClick={() => handleDelete(cat)}
                          className="bg-red-100 text-red-700 text-xs font-semibold px-3 py-1 rounded-full hover:bg-red-200 transition"
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