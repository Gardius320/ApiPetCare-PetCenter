"use client"

import { useState } from "react"
import {  Dialog, DialogContent, DialogFooter,  DialogHeader, DialogTitle,} from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue,} from "@/components/ui/select"
import { useAllOwners } from "@/features/owners/hooks/use-owners"
import type { CreatePetDto } from "../types/pet.types"
import { Dog, Cat, Bird, Rabbit,PawPrint } from "lucide-react"

const SPECIES_OPTIONS = [
  { id: 1, label: "Perro",  Icon: Dog },
  { id: 2, label: "Gato",   Icon: Cat },
  { id: 3, label: "Ave",    Icon: Bird },
  { id: 4, label: "Conejo", Icon: Rabbit },
  { id: 5, label: "Otro",   Icon: PawPrint },
]

interface PetFormModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  isSaving: boolean
  onSave: (dto: CreatePetDto) => void
}

export function PetFormModal({ open, onOpenChange, isSaving, onSave }: PetFormModalProps) {
  const [nombre, setNombre]       = useState("")
  const [especieId, setEspecieId] = useState("")
  const [ownerId, setOwnerId]     = useState("")

  const { data, isLoading: loadingOwners } = useAllOwners()
const owners = data?.items ?? []

  const isValid =
    nombre.trim() !== "" &&
    especieId !== "" &&
    ownerId !== ""

  function reset() {
    setNombre("")
    setEspecieId("")
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
      nombre: nombre.trim(),
      especieId: Number(especieId),
      propietarioId: Number(ownerId),
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
            <Label htmlFor="pet-nombre">Nombre</Label>
            <Input
              id="pet-nombre"
              placeholder="Ej. Firulais"
              value={nombre}
              onChange={(e) => setNombre(e.target.value)}
              autoComplete="off"
            />
          </div>

          {/* Especie */}
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="pet-especie">Especie</Label>
            <Select value={especieId} onValueChange={setEspecieId}>
              <SelectTrigger id="pet-especie" className="w-full">
                <SelectValue placeholder="Selecciona la especie" />
              </SelectTrigger>
              <SelectContent>
                {SPECIES_OPTIONS.map((s) => (
                <SelectItem key={s.id} value={String(s.id)}>
                 <span className="flex items-center gap-2">
                  <s.Icon className="h-4 w-4" />
                  {s.label}
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