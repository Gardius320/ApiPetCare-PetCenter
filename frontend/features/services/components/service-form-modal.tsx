"use client"

import { useState } from "react"
import { Wrench } from "lucide-react"
import { Dialog, DialogContent, DialogFooter } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Label } from "@/components/ui/label"
import { FormDialogHeader } from "@/components/shared/form-dialog-header"
import type { Service, CreateServiceDto } from "../types/service.types"

interface ServiceFormModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  isSaving: boolean
  onSave: (dto: CreateServiceDto, id?: number) => void
  service?: Service
}

export function ServiceFormModal({ open, onOpenChange, isSaving, onSave, service }: ServiceFormModalProps) {
  const isEditing = !!service

  const [name, setName]               = useState(service?.name ?? "")
  const [description, setDescription] = useState(service?.description ?? "")
  const [price, setPrice]             = useState(service?.price?.toString() ?? "")
  const [wasOpen, setWasOpen]         = useState(open)

  if (open && !wasOpen) {
    setWasOpen(true)
    setName(service?.name ?? "")
    setDescription(service?.description ?? "")
    setPrice(service?.price?.toString() ?? "")
  } else if (!open && wasOpen) {
    setWasOpen(false)
  }

  const isValid = name.trim() !== "" && price.trim() !== "" && Number(price) >= 0

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!isValid) return

    const dto: CreateServiceDto = {
      name: name.trim(),
      description: description.trim() || null,
      price: Number(price),
    }

    onSave(dto, service?.id)
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-sm">
        <FormDialogHeader
          icon={Wrench}
          title={isEditing ? "Editar servicio" : "Nuevo servicio"}
          accent="#1F6F5C"
        />

        <form onSubmit={handleSubmit} className="flex flex-col gap-4 py-1">

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="service-name">Nombre</Label>
            <Input
              id="service-name"
              placeholder="Ej. Consulta general"
              value={name}
              onChange={(e) => setName(e.target.value)}
              autoComplete="off"
            />
          </div>

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="service-description">Descripción</Label>
            <Textarea
              id="service-description"
              placeholder="Opcional"
              value={description ?? ""}
              onChange={(e) => setDescription(e.target.value)}
            />
          </div>

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="service-price">Precio</Label>
            <Input
              id="service-price"
              type="number"
              min="0"
              step="0.01"
              placeholder="0"
              value={price}
              onChange={(e) => setPrice(e.target.value)}
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
              {isSaving ? "Guardando..." : isEditing ? "Guardar cambios" : "Crear servicio"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}
