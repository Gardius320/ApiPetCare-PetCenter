"use client"

import { useState } from "react"
import { Users } from "lucide-react"
import {
  Dialog, DialogContent, DialogFooter,
} from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import {
  Select, SelectContent, SelectItem,
  SelectTrigger, SelectValue,
} from "@/components/ui/select"
import { FormDialogHeader } from "@/components/shared/form-dialog-header"
import { GENDER, type Owner, type CreateOwnerDto, type UpdateOwnerDto } from "../types/owner.types"

interface OwnerFormModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  isSaving: boolean
  onSave: (dto: CreateOwnerDto | UpdateOwnerDto, id?: number) => void
  owner?: Owner
}

export function OwnerFormModal({ open, onOpenChange, isSaving, onSave, owner }: OwnerFormModalProps) {
  const isEditing = !!owner

  const [ownerName, setOwnerName]     = useState(owner?.ownerName ?? "")
  const [email, setEmail]             = useState(owner?.email ?? "")
  const [phoneNumber, setPhoneNumber] = useState(owner?.phoneNumber ?? "")
  const [idCard, setIdCard]           = useState(owner?.idCard ?? "")
  const [gender, setGender]           = useState(owner?.gender ?? "")
  const [wasOpen, setWasOpen]         = useState(open)

  if (open && !wasOpen) {
    setWasOpen(true)
    setOwnerName(owner?.ownerName ?? "")
    setEmail(owner?.email ?? "")
    setPhoneNumber(owner?.phoneNumber ?? "")
    setIdCard(owner?.idCard ?? "")
    setGender(owner?.gender ?? "")
  } else if (!open && wasOpen) {
    setWasOpen(false)
  }

  const isValid =
    ownerName.trim() !== "" &&
    email.trim() !== "" &&
    phoneNumber.trim() !== "" &&
    gender !== "" &&
    (isEditing || (idCard.trim().length >= 7 && idCard.trim().length <= 10))

  function reset() {
    setOwnerName(owner?.ownerName ?? "")
    setEmail(owner?.email ?? "")
    setPhoneNumber(owner?.phoneNumber ?? "")
    setIdCard(owner?.idCard ?? "")
    setGender(owner?.gender ?? "")
  }

  function handleOpenChange(isOpen: boolean) {
    if (!isOpen) reset()
    onOpenChange(isOpen)
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!isValid) return

    if (isEditing) {
      onSave(
        {
          ownerName: ownerName.trim(),
          email: email.trim(),
          phoneNumber: phoneNumber.trim(),
          gender,
          idCard: idCard.trim(),
        },
        owner?.id
      )
    } else {
      onSave({
        ownerName: ownerName.trim(),
        email: email.trim(),
        phoneNumber: phoneNumber.trim(),
        idCard: idCard.trim(),
        gender,
      })
    }
  }

  return (
    <Dialog open={open} onOpenChange={handleOpenChange}>
      <DialogContent className="sm:max-w-sm">
        <FormDialogHeader
          icon={Users}
          title={isEditing ? "Editar propietario" : "Nuevo propietario"}
          accent="#FF8C6B"
        />

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
              value={phoneNumber}
              onChange={(e) => setPhoneNumber(e.target.value)}
              autoComplete="off"
            />
          </div>

          {/* Cédula — solo al crear, el backend la exige en Create pero no en Update */}
          {!isEditing && (
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="owner-idcard">Cédula</Label>
              <Input
                id="owner-idcard"
                placeholder="Ej. 1020304050"
                value={idCard}
                onChange={(e) => setIdCard(e.target.value)}
                maxLength={10}
                autoComplete="off"
              />
            </div>
          )}

          {/* Género */}
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="owner-gender">Género</Label>
            <Select value={gender} onValueChange={setGender}>
              <SelectTrigger id="owner-gender" className="w-full">
                <SelectValue placeholder="Selecciona el género" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value={GENDER.MALE}>♂ {GENDER.MALE}</SelectItem>
                <SelectItem value={GENDER.FEMALE}>♀ {GENDER.FEMALE}</SelectItem>
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
              className="bg-primary text-primary-foreground hover:bg-primary/90"
            >
              {isSaving ? "Guardando..." : isEditing ? "Guardar cambios" : "Crear propietario"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}