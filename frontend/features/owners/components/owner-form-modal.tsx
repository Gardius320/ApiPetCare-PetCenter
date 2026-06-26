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
import type { Owner, CreateOwnerDto, UpdateOwnerDto } from "../types/owner.types"

interface OwnerFormModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  isSaving: boolean
  onSave: (dto: CreateOwnerDto | UpdateOwnerDto, id?: number) => void
  owner?: Owner
}

export function OwnerFormModal({ open, onOpenChange, isSaving, onSave, owner }: OwnerFormModalProps) {
  const isEditing = !!owner

  const [ownerName, setOwnerName] = useState(owner?.ownerName ?? "")
  const [email, setEmail]         = useState(owner?.email ?? "")
  const [phone, setPhone]         = useState(owner?.phone ?? "")
  const [gender, setGender]       = useState(owner?.gender ?? "")

  const isValid =
    ownerName.trim() !== "" &&
    email.trim() !== "" &&
    phone.trim() !== "" &&
    gender !== ""

  function reset() {
    setOwnerName(owner?.ownerName ?? "")
    setEmail(owner?.email ?? "")
    setPhone(owner?.phone ?? "")
    setGender(owner?.gender ?? "")
  }

  function handleOpenChange(isOpen: boolean) {
    if (!isOpen) reset()
    onOpenChange(isOpen)
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!isValid) return
    onSave({
      ownerName: ownerName.trim(),
      email: email.trim(),
      phone: phone.trim(),
      gender,
    }, owner?.id)
  }

  return (
    <Dialog open={open} onOpenChange={handleOpenChange}>
      <DialogContent className="sm:max-w-sm">
        <DialogHeader>
          <DialogTitle>
            {isEditing ? "Editar propietario" : "Nuevo propietario"}
          </DialogTitle>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="flex flex-col gap-4 py-1">

          {/* Nombre */}
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="owner-name">Nombre completo</Label>
            <Input
              id="owner-name"
              placeholder="Ej. Juan Pérez"
              value={ownerName}
              onChange={(e) => setOwnerName(e.target.value)}
              autoComplete="off"
            />
          </div>

          {/* Email */}
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="owner-email">Correo electrónico</Label>
            <Input
              id="owner-email"
              type="email"
              placeholder="juan@ejemplo.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              autoComplete="off"
            />
          </div>

          {/* Teléfono */}
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="owner-phone">Teléfono</Label>
            <Input
              id="owner-phone"
              type="tel"
              placeholder="Ej. 300 123 4567"
              value={phone}
              onChange={(e) => setPhone(e.target.value)}
              autoComplete="off"
            />
          </div>

          {/* Género */}
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="owner-gender">Género</Label>
            <Select value={gender} onValueChange={setGender}>
              <SelectTrigger id="owner-gender" className="w-full">
                <SelectValue placeholder="Selecciona el género" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="Masculino">♂ Masculino</SelectItem>
                <SelectItem value="Femenino">♀ Femenino</SelectItem>
              </SelectContent>
            </Select>
          </div>

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => handleOpenChange(false)} disabled={isSaving}>
              Cancelar
            </Button>
            <Button 
            type="submit" 
            disabled={!isValid || isSaving}
            className="bg-blue-400 hover:bg-blue-500 text-white"
            >
              {isSaving ? "Guardando..." : isEditing ? "Guardar cambios" : "Crear propietario"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}