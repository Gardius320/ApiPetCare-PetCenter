"use client"

import { useState } from "react"
import { DialogFooter } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { useVeterinarians } from "@/features/users/hooks/use-users"
import { useAppointmentsByPetId } from "@/features/appointments/hooks/use-appointments"
import type { MedicalRecord, CreateMedicalRecordPayload, UpdateMedicalRecordPayload } from "../types/medical-record.types"

interface MedicalRecordFormFieldsProps {
  isSaving: boolean
  onSave: (dto: CreateMedicalRecordPayload | UpdateMedicalRecordPayload, id?: number) => void
  onCancel: () => void
  petId: number
  record?: MedicalRecord
}

export function MedicalRecordFormFields({
  isSaving, onSave, onCancel, petId, record,
}: MedicalRecordFormFieldsProps) {
  const isEditing = !!record

  const { data: veterinarians, isLoading: loadingVets } = useVeterinarians()
  const { data: appointmentsData, isLoading: loadingAppointments } = useAppointmentsByPetId(petId)

  const rawAppointments = appointmentsData?.items ?? []
  const completedAppointments = rawAppointments.filter((a) => a.state === "Completada")

  // Si estamos editando y la cita ya asociada al registro no pasa el filtro
  // (por ejemplo, cambió de estado después de crear el historial), la agregamos
  // igual a las opciones para no romper la edición ni mostrar el select vacío.
  const currentAppointmentMissing =
    isEditing &&
    record?.appointmentId != null &&
    !completedAppointments.some((a) => a.id === record.appointmentId)

  const currentAppointment = currentAppointmentMissing
    ? rawAppointments.find((a) => a.id === record!.appointmentId)
    : undefined

  const appointments = currentAppointment
    ? [...completedAppointments, currentAppointment]
    : completedAppointments

  const [appointmentId, setAppointmentId]     = useState<number | null>(record?.appointmentId ?? null)
  const [veterinarianUserId, setVeterinarianUserId] = useState(record?.veterinarianUserId ?? "")
  const [visitDate, setVisitDate]             = useState(record?.visitDate.slice(0, 16) ?? new Date().toISOString().slice(0, 16))
  const [diagnosis, setDiagnosis]             = useState(record?.diagnosis ?? "")
  const [treatment, setTreatment]             = useState(record?.treatment ?? "")
  const [weight, setWeight]                   = useState(record?.weight?.toString() ?? "")
  const [temperature, setTemperature]         = useState(record?.temperature?.toString() ?? "")
  const [observations, setObservations]       = useState(record?.observations ?? "")
  const [nextFollowUpDate, setNextFollowUpDate] = useState(record?.nextFollowUpDate?.slice(0, 16) ?? "")

  const isValid =
    veterinarianUserId !== "" &&
    visitDate !== "" &&
    diagnosis.trim() !== "" &&
    treatment.trim() !== ""

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!isValid) return

    const basePayload: CreateMedicalRecordPayload = {
      petId,
      appointmentId,
      veterinarianUserId,
      visitDate,
      diagnosis: diagnosis.trim(),
      treatment: treatment.trim(),
      weight: weight ? Number(weight) : null,
      temperature: temperature ? Number(temperature) : null,
      observations: observations.trim() || null,
      nextFollowUpDate: nextFollowUpDate || null,
    }

    if (isEditing) {
      onSave({ ...basePayload, id: record.id }, record.id)
    } else {
      onSave(basePayload)
    }
  }

  return (
    <>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4 py-1">

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="mr-appointment">Cita asociada (opcional)</Label>
          <Select
            value={appointmentId?.toString() ?? "none"}
            onValueChange={(v) => {
              setAppointmentId(v === "none" ? null : Number(v))
            }}
          >
            <SelectTrigger id="mr-appointment" className="w-full">
              <SelectValue placeholder={loadingAppointments ? "Cargando citas..." : "Sin cita asociada"} />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="none">Sin cita asociada</SelectItem>
              {appointments.map((a) => (
                <SelectItem key={a.id} value={String(a.id)}>
                  {new Date(a.date).toLocaleDateString("es-CO")} — {a.observation || "Sin observación"}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="mr-vet">Veterinario</Label>
          <Select
            value={veterinarianUserId}
            onValueChange={(v) => {
              setVeterinarianUserId(v)
            }}
          >
            <SelectTrigger id="mr-vet" className="w-full">
              <SelectValue placeholder={loadingVets ? "Cargando..." : "Selecciona un veterinario"} />
            </SelectTrigger>
            <SelectContent>
              {veterinarians?.map((vet) => (
                <SelectItem key={vet.id} value={vet.id}>
                  {vet.fullName}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="mr-visit-date">Fecha de la visita</Label>
          <Input
            id="mr-visit-date"
            type="datetime-local"
            value={visitDate}
            onChange={(e) => setVisitDate(e.target.value)}
          />
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="mr-diagnosis">Diagnóstico</Label>
          <Textarea
            id="mr-diagnosis"
            value={diagnosis}
            onChange={(e) => setDiagnosis(e.target.value)}
            maxLength={500}
          />
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="mr-treatment">Tratamiento</Label>
          <Textarea
            id="mr-treatment"
            value={treatment}
            onChange={(e) => setTreatment(e.target.value)}
            maxLength={500}
          />
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="mr-weight">Peso (kg)</Label>
            <Input
              id="mr-weight"
              type="number"
              step="0.01"
              value={weight}
              onChange={(e) => setWeight(e.target.value)}
            />
          </div>
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="mr-temp">Temperatura (°C)</Label>
            <Input
              id="mr-temp"
              type="number"
              step="0.01"
              value={temperature}
              onChange={(e) => setTemperature(e.target.value)}
            />
          </div>
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="mr-observations">Observaciones</Label>
          <Textarea
            id="mr-observations"
            value={observations}
            onChange={(e) => setObservations(e.target.value)}
            maxLength={500}
          />
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="mr-followup">Próximo control sugerido</Label>
          <Input
            id="mr-followup"
            type="datetime-local"
            value={nextFollowUpDate}
            onChange={(e) => setNextFollowUpDate(e.target.value)}
          />
        </div>

        <DialogFooter>
          <Button type="button" variant="outline" onClick={onCancel} disabled={isSaving}>
            Cancelar
          </Button>
          <Button type="submit" disabled={!isValid || isSaving} className="bg-[#5B8CFF] text-white hover:bg-[#4a76e0]">
            {isSaving ? "Guardando..." : isEditing ? "Guardar cambios" : "Crear entrada"}
          </Button>
        </DialogFooter>
      </form>
    </>
  )
}