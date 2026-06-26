"use client"

import { useState, useEffect } from "react"
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
  const pets   = petsData?.items.filter(p => p.propietarioId === Number(ownerId)) ?? []

  const createAppointment = useCreateAppointment()
  const updateAppointment = useUpdateAppointment()

  const isEditing = !!selected

  useEffect(() => {
  if (!open) return
  if (selected) {
    const fecha = selected.fecha ? new Date(selected.fecha).toISOString().slice(0, 16) : ""
    setDate(fecha)
    setObservation(selected.observacion ?? "")
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

  if (!open) return null

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40">
      <div className="bg-white rounded-2xl shadow-xl p-6 w-full max-w-md space-y-4">

        <h2 className="text-lg font-bold text-gray-800">
          {isEditing ? "Editar Cita" : "Nueva Cita"}
        </h2>

        {/* Dueño — solo en creación */}
        {!isEditing && (
          <div className="space-y-1">
            <label className="text-sm text-gray-600">Propietario</label>
            <select
              value={ownerId}
              onChange={(e) => { setOwnerId(e.target.value); setPetId("") }}
              className="w-full border border-gray-200 rounded-lg px-3 py-2 text-sm"
            >
              <option value="">Selecciona un propietario</option>
              {owners.map((o) => (
                <option key={o.id} value={o.id}>{o.ownerName}</option>
              ))}
            </select>
          </div>
        )}

        {/* Mascota — solo en creación */}
        {!isEditing && (
          <div className="space-y-1">
            <label className="text-sm text-gray-600">Mascota</label>
            <select
              value={petId}
              onChange={(e) => setPetId(e.target.value)}
              disabled={!ownerId}
              className="w-full border border-gray-200 rounded-lg px-3 py-2 text-sm disabled:opacity-50"
            >
              <option value="">Selecciona una mascota</option>
              {pets.map((p) => (
                <option key={p.id} value={p.id}>{p.nombre}</option>
              ))}
            </select>
          </div>
        )}

        {/* Fecha */}
        <div className="space-y-1">
          <label className="text-sm text-gray-600">Fecha y hora</label>
          <input
            type="datetime-local"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            className="w-full border border-gray-200 rounded-lg px-3 py-2 text-sm"
          />
        </div>

        {/* Observación */}
        <div className="space-y-1">
          <label className="text-sm text-gray-600">Observación</label>
          <textarea
            value={observation}
            onChange={(e) => setObservation(e.target.value)}
            rows={3}
            className="w-full border border-gray-200 rounded-lg px-3 py-2 text-sm resize-none"
          />
        </div>

        {/* Botones */}
        <div className="flex justify-end gap-2 pt-2">
          <button
            onClick={() => onOpenChange(false)}
            className="px-4 py-2 text-sm rounded-lg border border-gray-200 hover:bg-gray-50 transition"
          >
            Cancelar
          </button>
          <button
            onClick={handleSave}
            disabled={isEditing ? !date || isPending : !ownerId || !petId || !date || isPending}
            className="px-4 py-2 text-sm rounded-lg bg-blue-400 text-white hover:opacity-90 transition disabled:opacity-50"
          >
            {isPending ? "Guardando..." : isEditing ? "Actualizar cita" : "Guardar cita"}
          </button>
        </div>

      </div>
    </div>
  )
}