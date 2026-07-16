"use client"

import { useState } from "react"
import {
  Dialog, DialogContent, DialogFooter,
  DialogHeader, DialogTitle,
} from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import {
  Select, SelectContent, SelectItem,
  SelectTrigger, SelectValue,
} from "@/components/ui/select"
import { useAllSupplyCategories } from "../../supply-categories/hooks/use-supply-categories"
import type { SupplyCategory } from "../../supply-categories/types/supply-category.types"
import type { Supply, CreateSupplyDto, UpdateSupplyDto } from "../types/supply.types"

interface SupplyFormModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  isSaving: boolean
  onSave: (dto: CreateSupplyDto | UpdateSupplyDto, id?: number) => void
  supply?: Supply
}

export function SupplyFormModal({ open, onOpenChange, isSaving, onSave, supply }: SupplyFormModalProps) {
  const isEditing = !!supply

  const { data: categories, isLoading: loadingCategories } = useAllSupplyCategories()

  const [name, setName]                 = useState(supply?.name ?? "")
  const [description, setDescription]   = useState(supply?.description ?? "")
  const [unit, setUnit]                 = useState(supply?.unit ?? "")
  const [currentStock, setCurrentStock] = useState(supply?.currentStock?.toString() ?? "")
  const [minimumStock, setMinimumStock] = useState(supply?.minimumStock?.toString() ?? "")
  const [categoryId, setCategoryId]     = useState(supply?.supplyCategoryId?.toString() ?? "")
  const [isActive, setIsActive]         = useState(supply?.isActive ?? true)
  const [wasOpen, setWasOpen]           = useState(open)

  if (open && !wasOpen) {
    setWasOpen(true)
    setName(supply?.name ?? "")
    setDescription(supply?.description ?? "")
    setUnit(supply?.unit ?? "")
    setCurrentStock(supply?.currentStock?.toString() ?? "")
    setMinimumStock(supply?.minimumStock?.toString() ?? "")
    setCategoryId(supply?.supplyCategoryId?.toString() ?? "")
    setIsActive(supply?.isActive ?? true)
  } else if (!open && wasOpen) {
    setWasOpen(false)
  }

  const isValid =
    name.trim() !== "" &&
    unit.trim() !== "" &&
    currentStock !== "" &&
    minimumStock !== "" &&
    categoryId !== ""

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!isValid) return

    const base = {
      name: name.trim(),
      description: description.trim() || undefined,
      unit: unit.trim(),
      currentStock: Number(currentStock),
      minimumStock: Number(minimumStock),
      supplyCategoryId: Number(categoryId),
    }

    if (isEditing) {
      onSave({ ...base, isActive } as UpdateSupplyDto, supply.id)
    } else {
      onSave(base as CreateSupplyDto)
    }
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-sm">
        <DialogHeader>
          <DialogTitle>
            {isEditing ? "Editar insumo" : "Nuevo insumo"}
          </DialogTitle>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="flex flex-col gap-4 py-1">

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="supply-name">Nombre</Label>
            <Input
              id="supply-name"
              placeholder="Ej. Guantes de nitrilo"
              value={name}
              onChange={(e) => setName(e.target.value)}
              autoComplete="off"
            />
          </div>

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="supply-description">Descripción</Label>
            <Input
              id="supply-description"
              placeholder="Opcional"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              autoComplete="off"
            />
          </div>

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="supply-unit">Unidad de medida</Label>
            <Input
              id="supply-unit"
              placeholder="Ej. cajas, ml, unidades"
              value={unit}
              onChange={(e) => setUnit(e.target.value)}
              autoComplete="off"
            />
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="supply-current-stock">Stock actual</Label>
              <Input
                id="supply-current-stock"
                type="number"
                min="0"
                step="0.01"
                value={currentStock}
                onChange={(e) => setCurrentStock(e.target.value)}
              />
            </div>
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="supply-minimum-stock">Stock mínimo</Label>
              <Input
                id="supply-minimum-stock"
                type="number"
                min="0"
                step="0.01"
                value={minimumStock}
                onChange={(e) => setMinimumStock(e.target.value)}
              />
            </div>
          </div>

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="supply-category">Categoría</Label>
            <Select value={categoryId} onValueChange={setCategoryId}>
              <SelectTrigger id="supply-category" className="w-full">
                <SelectValue placeholder={loadingCategories ? "Cargando..." : "Selecciona una categoría"} />
              </SelectTrigger>
              <SelectContent>
                {categories?.map((cat: SupplyCategory) => (
                  <SelectItem key={cat.id} value={cat.id.toString()}>
                    {cat.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          {isEditing && (
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="supply-status">Estado</Label>
              <Select value={isActive ? "true" : "false"} onValueChange={(v) => setIsActive(v === "true")}>
                <SelectTrigger id="supply-status" className="w-full">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="true">Activo</SelectItem>
                  <SelectItem value="false">Inactivo</SelectItem>
                </SelectContent>
              </Select>
            </div>
          )}

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)} disabled={isSaving}>
              Cancelar
            </Button>
            <Button
              type="submit"
              disabled={!isValid || isSaving}
              className="bg-blue-400 hover:bg-blue-500 text-white"
            >
              {isSaving ? "Guardando..." : isEditing ? "Guardar cambios" : "Crear insumo"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}