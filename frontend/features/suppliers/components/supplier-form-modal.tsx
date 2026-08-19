"use client"

import { useState } from "react"
import { Truck } from "lucide-react"
import {
  Dialog, DialogContent, DialogFooter,
} from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { FormDialogHeader } from "@/components/shared/form-dialog-header"
import type { Supplier, CreateSupplierDto } from "../types/supplier.types"

interface SupplierFormModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  isSaving: boolean
  onSave: (dto: CreateSupplierDto, id?: number) => void
  supplier?: Supplier
}

export function SupplierFormModal({
  open, onOpenChange, isSaving, onSave, supplier,
}: SupplierFormModalProps) {
  const isEditing = !!supplier

  const [name, setName] = useState(supplier?.name ?? "")
  const [contactNumber, setContactNumber] = useState(supplier?.contactNumber ?? "")
  const [email, setEmail] = useState(supplier?.email ?? "")
  const [address, setAddress] = useState(supplier?.address ?? "")
  const [description, setDescription] = useState(supplier?.description ?? "")
  const [wasOpen, setWasOpen] = useState(open)

  if (open && !wasOpen) {
    setWasOpen(true)
    setName(supplier?.name ?? "")
    setContactNumber(supplier?.contactNumber ?? "")
    setEmail(supplier?.email ?? "")
    setAddress(supplier?.address ?? "")
    setDescription(supplier?.description ?? "")
  } else if (!open && wasOpen) {
    setWasOpen(false)
  }

  const isValid = name.trim() !== ""

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!isValid) return

    const dto: CreateSupplierDto = {
      name: name.trim(),
      contactNumber: contactNumber.trim() || null,
      email: email.trim() || null,
      address: address.trim() || null,
      description: description.trim() || null,
    }

    onSave(dto, supplier?.id)
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-sm">
        <FormDialogHeader
          icon={Truck}
          title={isEditing ? "Editar proveedor" : "Nuevo proveedor"}
          accent="#FF8C6B"
        />

        <form onSubmit={handleSubmit} className="flex flex-col gap-4 py-1">
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="supplier-name">Nombre</Label>
            <Input
              id="supplier-name"
              placeholder="Ej. Distribuidora VetSalud"
              value={name}
              onChange={(e) => setName(e.target.value)}
              autoComplete="off"
            />
          </div>

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="supplier-contact">Teléfono de contacto</Label>
            <Input
              id="supplier-contact"
              placeholder="Opcional"
              value={contactNumber ?? ""}
              onChange={(e) => setContactNumber(e.target.value)}
              autoComplete="off"
            />
          </div>

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="supplier-email">Email</Label>
            <Input
              id="supplier-email"
              type="email"
              placeholder="Opcional"
              value={email ?? ""}
              onChange={(e) => setEmail(e.target.value)}
              autoComplete="off"
            />
          </div>

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="supplier-address">Dirección</Label>
            <Input
              id="supplier-address"
              placeholder="Opcional"
              value={address ?? ""}
              onChange={(e) => setAddress(e.target.value)}
              autoComplete="off"
            />
          </div>

          <div className="flex flex-col gap-1.5">
            <Label htmlFor="supplier-description">Qué productos trae</Label>
            <Textarea
              id="supplier-description"
              placeholder="Ej. Medicamentos, vacunas, insumos de aseo..."
              value={description ?? ""}
              onChange={(e) => setDescription(e.target.value)}
              rows={2}
            />
          </div>

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)} disabled={isSaving}>
              Cancelar
            </Button>
            <Button
              type="submit"
              disabled={!isValid || isSaving}
              className="bg-primary text-primary-foreground hover:bg-primary/90"
            >
              {isSaving ? "Guardando..." : isEditing ? "Guardar cambios" : "Crear proveedor"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}