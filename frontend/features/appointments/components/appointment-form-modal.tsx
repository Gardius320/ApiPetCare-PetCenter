"use client"

import { useState, useEffect } from "react"
import { Calendar } from "lucide-react"
import { Dialog, DialogContent, DialogFooter } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { FormDialogHeader } from "@/components/shared/form-dialog-header"
import { useCreateAppointment, useUpdateAppointment } from "../hooks/use-appointments"
import { useAllOwners } from "@/features/owners/hooks/use-owners"
import { usePets } from "@/features/pets/hooks/use-pets"
import type { Appointment } from "../types/appointment.types"

interface Props {
  open: boolean
  onOpenChange: (open: boolean) => void
  selected?: Appointment | null
}

export function AppointmentFormModal({ open, onOpenChange, selected }: Props) {
  const [ownerId, setOwnerId]         = useState("")
  const [petId, setPetId]             = useState("")
  const [date, setDate]               = useState("")
  const [observation, setObservation] = useState("")

  const { data: ownersData } = useAllOwners()
  const { data: petsData }   = usePets(1, 100, "")

  const owners = ownersData?.items ?? []
  const pets   = petsData?.items.filter(p => p.ownerId === Number(ownerId)) ?? []

  const createAppointment = useCreateAppointment()
  const updateAppointment = useUpdateAppointment()

  const isEditing = !!selected

  useEffect(() => {
  if (!open) return
  if (selected) {
    const formattedDate = selected.date ? new Date(selected.date).toISOString().slice(0, 16) : ""
    // eslint-disable-next-line react-hooks/set-state-in-effect
    setDate(formattedDate)
    setObservation(selected.observation ?? "")
    setOwnerId("")
    setPetId("")
  } else {
    setDate("")
    setObservation("")
    setOwnerId("")
    setPetId("")
  }
}, [selected, open])

  const handleSave = () => {
    if (isEditing) {
      if (!date) return
      updateAppointment.mutate(
        { id: selected.id, appointmentDate: date, observation },
        { onSuccess: () => onOpenChange(false) }
      )
    } else {
      if (!ownerId || !petId || !date) return
      createAppointment.mutate(
        { ownerId: Number(ownerId), petId: Number(petId), appointmentDate: date, observation },
        { onSuccess: () => {
            onOpenChange(false)
            setOwnerId(""); setPetId(""); setDate(""); setObservation("")
          }
        }
      )
    }
  }

  const isPending = createAppointment.isPending || updateAppointment.isPending

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-sm">
        <FormDialogHeader
          icon={Calendar}
          title={isEditing ? "Editar cita" : "Nueva cita"}
          accent="#E0A458"
        />

        <div className="flex flex-col gap-4 py-1">
          {/* Dueño — solo en creación */}
          {!isEditing && (
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="appointment-owner">Propietario</Label>
              <Select value={ownerId} onValueChange={(v) => { setOwnerId(v); setPetId("") }}>
                <SelectTrigger id="appointment-owner" className="w-full">
                  <SelectValue placeholder="Selecciona un propietario" />
                </SelectTrigger>
                <SelectContent>
                  {owners.map((o) => (
                    <SelectItem key={o.id} value={String(o.id)}>{o.ownerName}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          )}

          {/* Mascota — solo en creación */}
          {!isEditing && (
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="appointment-pet">Mascota</Label>
              <Select value={petId} onValueChange={setPetId} disabled={!ownerId}>
                <SelectTrigger id="appointment-pet" className="w-full">
                  <SelectValue placeholder="Selecciona una mascota" />
                </SelectTrigger>
                <SelectContent>
                  {pets.map((p) => (
                    <SelectItem key={p.id} value={String(p.id)}>{p.name}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          )}

          {/* Fecha */}
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="appointment-date">Fecha y hora</Label>
            <Input
              id="appointment-date"
              type="datetime-local"
              value={date}
              onChange={(e) => setDate(e.target.value)}
            />
          </div>

          {/* Observación */}
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="appointment-observation">Observación</Label>
            <Textarea
              id="appointment-observation"
              value={observation}
              onChange={(e) => setObservation(e.target.value)}
              rows={3}
            />
          </div>
        </div>

        <DialogFooter>
          <Button type="button" variant="outline" onClick={() => onOpenChange(false)} disabled={isPending}>
            Cancelar
          </Button>
          <Button
            type="button"
            onClick={handleSave}
            disabled={isEditing ? !date || isPending : !ownerId || !petId || !date || isPending}
            className="bg-[#1F6F5C] text-white hover:bg-[#18594a]"
          >
            {isPending ? "Guardando..." : isEditing ? "Actualizar cita" : "Guardar cita"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
