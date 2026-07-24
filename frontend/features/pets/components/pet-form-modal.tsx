"use client"

import { useState } from "react"
import {  Dialog, DialogContent, DialogFooter,  DialogHeader, DialogTitle,} from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue,} from "@/components/ui/select"
import { useAllOwners } from "@/features/owners/hooks/use-owners"
import { useSpecies } from "@/features/species/hooks/use-species"
import type { CreatePetDto } from "../types/pet.types"
import { PawPrint } from "lucide-react"

interface PetFormModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  isSaving: boolean
  onSave: (dto: CreatePetDto) => void
}

export function PetFormModal({ open, onOpenChange, isSaving, onSave }: PetFormModalProps) {
  const [name, setName]         = useState("")
  const [specieId, setSpecieId] = useState("")
  const [ownerId, setOwnerId]   = useState("")

  const { data, isLoading: loadingOwners } = useAllOwners()
const owners = data?.items ?? []

  const { data: species, isLoading: loadingSpecies } = useSpecies()

  const isValid =
    name.trim() !== "" &&
    specieId !== "" &&
    ownerId !== ""

  function reset() {
    setName("")
    setSpecieId("")
    setOwnerId("")
  }

  function handleClose(isOpen: boolean) {
    if (!isOpen) reset()
    onOpenChange(isOpen)
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!isValid) return
    onSave({
      petName: name.trim(),
      specieId: Number(specieId),
      ownerId: Number(ownerId),
    })
  }

  return (
    <Dialog open={open} onOpenChange={handleClose}>
      <DialogContent className="sm:max-w-sm">
        <DialogHeader>
          <DialogTitle>Nueva mascota</DialogTitle>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="flex flex-col gap-4 py-1">

          {/* Nombre */}
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="pet-name">Nombre</Label>
            <Input
              id="pet-name"
              placeholder="Ej. Firulais"
              value={name}
              onChange={(e) => setName(e.target.value)}
              autoComplete="off"
            />
          </div>

          {/* Especie */}
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="pet-species">Especie</Label>
            <Select value={specieId} onValueChange={setSpecieId} disabled={loadingSpecies}>
              <SelectTrigger id="pet-species" className="w-full">
                <SelectValue placeholder={loadingSpecies ? "Cargando..." : "Selecciona la especie"} />
              </SelectTrigger>
              <SelectContent>
                {(species ?? []).map((s) => (
                <SelectItem key={s.id} value={String(s.id)}>
                 <span className="flex items-center gap-2">
                  <PawPrint className="h-4 w-4" />
                  {s.speciesName}
                    </span>
                     </SelectItem>
                          ))}
              </SelectContent>
            </Select>
          </div>

          {/* Propietario */}
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="pet-owner">Propietario</Label>
            <Select value={ownerId} onValueChange={setOwnerId} disabled={loadingOwners}>
              <SelectTrigger id="pet-owner" className="w-full">
                <SelectValue placeholder={loadingOwners ? "Cargando..." : "Selecciona el propietario"} />
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

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => handleClose(false)} disabled={isSaving}>
              Cancelar
            </Button>
            <Button 
              type="submit" 
               disabled={!isValid || isSaving}
                 className="bg-blue-400 hover:bg-blue-500 text-white"
                   >
                  {isSaving ? "Guardando..." : "Guardar mascota"}
                 </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}