"use client"

import { useState } from "react"
import { Layers } from "lucide-react"
import {
  Dialog, DialogContent, DialogFooter,
} from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { FormDialogHeader } from "@/components/shared/form-dialog-header"
import type { SupplyCategory, CreateSupplyCategoryDto } from "../types/supply-category.types"

interface SupplyCategoryFormModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  isSaving: boolean
  onSave: (dto: CreateSupplyCategoryDto, id?: number) => void
  category?: SupplyCategory
}

export function SupplyCategoryFormModal({
  open, onOpenChange, isSaving, onSave, category,
}: SupplyCategoryFormModalProps) {
  const isEditing = !!category

  const [name, setName] = useState(category?.name ?? "")
  const [description, setDescription] = useState(category?.description ?? "")
  const [wasOpen, setWasOpen] = useState(open)

  if (open && !wasOpen) {
    setWasOpen(true)
    setName(category?.name ?? "")
    setDescription(category?.description ?? "")
  } else if (!open && wasOpen) {
    setWasOpen(false)
  }

  const isValid = name.trim() !== ""

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!isValid) return

    const dto: CreateSupplyCategoryDto = {
      name: name.trim(),
      description: description.trim() || null,
    }

    onSave(dto, category?.id)
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-sm">
        <FormDialogHeader
          icon={Layers}
          title={isEditing ? "Editar categoría" : "Nueva categoría"}
          accent="#E0A458"
        />

        <form onSubmit={handleSubmit} className="flex flex-col gap-4 py-1">
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="category-name">Nombre</Label>
            <Input
              id="category-name"
              placeholder="Ej. Medicamentos"
              value={name}
              onChange={(e) => setName(e.target.value)}
              autoComplete="off"
            />
          </div>

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="category-description">Descripción</Label>
            <Input
              id="category-description"
              placeholder="Opcional"
              value={description ?? ""}
              onChange={(e) => setDescription(e.target.value)}
              autoComplete="off"
            />
          </div>

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)} disabled={isSaving}>
              Cancelar
            </Button>
            <Button
              type="submit"
              disabled={!isValid || isSaving}
              className="bg-[#1F6F5C] text-white hover:bg-[#18594a]"
            >
              {isSaving ? "Guardando..." : isEditing ? "Guardar cambios" : "Crear categoría"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}