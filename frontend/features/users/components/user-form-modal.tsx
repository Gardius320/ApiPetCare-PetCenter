"use client"

import { useState } from "react"
import { UserCog } from "lucide-react"
import { Dialog, DialogContent, DialogFooter } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { FormDialogHeader } from "@/components/shared/form-dialog-header"
import type { User, CreateUserDto, ChangeRoleDto } from "../types/user.types"

const ROLES = ["Admin", "Veterinarian", "Assistant"]

interface UserFormModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  isSaving: boolean
  onSave: (dto: CreateUserDto | ChangeRoleDto, id?: string) => void
  user?: User
}

export function UserFormModal({ open, onOpenChange, isSaving, onSave, user }: UserFormModalProps) {
  const isEditing = !!user

  const [email, setEmail] = useState(user?.email ?? "")
  const [firstName, setFirstName] = useState("")
  const [lastName, setLastName] = useState("")
  const [password, setPassword] = useState("")
  const [role, setRole] = useState(user?.role ?? "")

  const isValid = isEditing
    ? role !== ""
    : email.trim() !== "" && firstName.trim() !== "" && lastName.trim() !== "" && password.trim() !== "" && role !== ""

  function reset() {
    setEmail(user?.email ?? "")
    setFirstName("")
    setLastName("")
    setPassword("")
    setRole(user?.role ?? "")
  }

  function handleOpenChange(isOpen: boolean) {
    if (!isOpen) reset()
    onOpenChange(isOpen)
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!isValid) return

    if (isEditing) {
      onSave({ role }, user?.id)
    } else {
      onSave({ email, firstName, lastName, password, role })
    }
  }

  return (
    <Dialog open={open} onOpenChange={handleOpenChange}>
      <DialogContent>
        <FormDialogHeader
          icon={UserCog}
          title={isEditing ? "Cambiar rol" : "Nuevo usuario"}
          accent="#FF8C6B"
        />

        <form onSubmit={handleSubmit} className="space-y-4">
          {!isEditing && (
            <>
              <div className="space-y-2">
                <Label htmlFor="firstName">Nombre</Label>
                <Input
                  id="firstName"
                  value={firstName}
                  onChange={(e) => setFirstName(e.target.value)}
                  placeholder="Carlos"
                  disabled={isSaving}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="lastName">Apellido</Label>
                <Input
                  id="lastName"
                  value={lastName}
                  onChange={(e) => setLastName(e.target.value)}
                  placeholder="Rodriguez"
                  disabled={isSaving}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="email">Email</Label>
                <Input
                  id="email"
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  placeholder="correo@petcare.com"
                  disabled={isSaving}
                />
              </div>
              <div className="space-y-2">
  <Label htmlFor="password">Contraseña</Label>
  <Input
    id="password"
    type="password"
    value={password}
    onChange={(e) => setPassword(e.target.value)}
    placeholder="Mínimo 8 caracteres"
    disabled={isSaving}
  />
  <p className="text-xs text-muted-foreground">
    Debe tener mínimo 8 caracteres, una mayúscula, un número y un carácter especial (!@#$%)
  </p>
</div>
            </>
          )}

          <div className="space-y-2">
            <Label htmlFor="role">Rol</Label>
            <Select value={role} onValueChange={setRole} disabled={isSaving}>
              <SelectTrigger>
                <SelectValue placeholder="Selecciona un rol" />
              </SelectTrigger>
              <SelectContent>
                {ROLES.map((r) => (
                  <SelectItem key={r} value={r}>{r}</SelectItem>
                ))}
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
              className="bg-[#1F6F5C] text-white hover:bg-[#18594a]"
            >
              {isSaving ? "Guardando..." : isEditing ? "Actualizar Rol" : "Crear Usuario"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}