"use client"

import { useState } from "react"
import { Dialog, DialogContent, DialogFooter, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import type { Species, CreateSpeciesDto, UpdateSpeciesDto } from "../types/species.types"

interface SpeciesFormModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  isSaving: boolean
  onSave: (dto: CreateSpeciesDto | UpdateSpeciesDto, id?: number) => void
  species?: Species
}

export function SpeciesFormModal({ open, onOpenChange, isSaving, onSave, species }: SpeciesFormModalProps) {
  const isEditing = !!species

  const [speciesName, setSpeciesName] = useState(species?.speciesName ?? "")

  const isValid = speciesName.trim() !== ""

  function reset() {
    setSpeciesName(species?.speciesName ?? "")
  }

  function handleOpenChange(isOpen: boolean) {
    if (!isOpen) reset()
    onOpenChange(isOpen)
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!isValid) return
    onSave(
      { speciesName: speciesName.trim() },
      species?.id
    )
  }

  return (
    <Dialog open={open} onOpenChange={handleOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{isEditing ? "Editar Especie" : "Nueva Especie"}</DialogTitle>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="speciesName">Nombre de la especie</Label>
            <Input
              id="speciesName"
              value={speciesName}
              onChange={(e) => setSpeciesName(e.target.value)}
              placeholder="Ej: Perro, Gato, Ave..."
              disabled={isSaving}
            />
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={() => handleOpenChange(false)}
              disabled={isSaving}
            >
              Cancelar
            </Button>
            <Button type="submit" disabled={!isValid || isSaving}>
              {isSaving ? "Guardando..." : isEditing ? "Actualizar" : "Crear"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}