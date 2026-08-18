"use client"

import { useState } from "react"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import {
  Select, SelectContent, SelectItem,
  SelectTrigger, SelectValue,
} from "@/components/ui/select"
import { Button } from "@/components/ui/button"
import { GENDER } from "@/features/owners/types/owner.types"
import { useSpecies } from "@/features/species/hooks/use-species"
import type { BookOnlineDto } from "../types/booking.types"

interface Props {
  date: string
  slot: string | null
  onSubmit: (dto: BookOnlineDto) => void
  isSubmitting: boolean
}

export function BookingForm({ date, slot, onSubmit, isSubmitting }: Props) {
  const [ownerName, setOwnerName]     = useState("")
  const [email, setEmail]             = useState("")
  const [phoneNumber, setPhoneNumber] = useState("")
  const [gender, setGender]           = useState("")
  const [petName, setPetName]         = useState("")
  const [specieId, setSpecieId]       = useState("")
  const [observation, setObservation] = useState("")

  const { data: species } = useSpecies()

  const isValid =
    ownerName.trim() !== "" &&
    email.trim() !== "" &&
    phoneNumber.trim() !== "" &&
    gender !== "" &&
    petName.trim() !== "" &&
    specieId !== "" &&
    !!date &&
    !!slot

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!isValid || !slot) return

    onSubmit({
      ownerName: ownerName.trim(),
      email: email.trim(),
      phoneNumber: phoneNumber.trim(),
      gender,
      petName: petName.trim(),
      specieId: Number(specieId),
      observation: observation.trim() || undefined,
      appointmentDate: `${date}T${slot}:00`,
    })
  }

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-4">
      {/* Nombre */}
      <div className="flex flex-col gap-1.5">
        <Label htmlFor="booking-owner-name">Nombre completo</Label>
        <Input
          id="booking-owner-name"
          placeholder="Ej. Juan Pérez"
          value={ownerName}
          onChange={(e) => setOwnerName(e.target.value)}
          autoComplete="off"
        />
      </div>

      {/* Email */}
      <div className="flex flex-col gap-1.5">
        <Label htmlFor="booking-email">Correo electrónico</Label>
        <Input
          id="booking-email"
          type="email"
          placeholder="juan@ejemplo.com"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          autoComplete="off"
        />
      </div>

      {/* Teléfono */}
      <div className="flex flex-col gap-1.5">
        <Label htmlFor="booking-phone">Teléfono</Label>
        <Input
          id="booking-phone"
          type="tel"
          placeholder="Ej. 300 123 4567"
          value={phoneNumber}
          onChange={(e) => setPhoneNumber(e.target.value)}
          autoComplete="off"
        />
      </div>

      {/* Género */}
      <div className="flex flex-col gap-1.5">
        <Label htmlFor="booking-gender">Género</Label>
        <Select value={gender} onValueChange={setGender}>
          <SelectTrigger id="booking-gender" className="w-full">
            <SelectValue placeholder="Selecciona el género" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value={GENDER.MALE}>♂ {GENDER.MALE}</SelectItem>
            <SelectItem value={GENDER.FEMALE}>♀ {GENDER.FEMALE}</SelectItem>
          </SelectContent>
        </Select>
      </div>

      {/* Nombre de la mascota */}
      <div className="flex flex-col gap-1.5">
        <Label htmlFor="booking-pet-name">Nombre de tu mascota</Label>
        <Input
          id="booking-pet-name"
          placeholder="Ej. Firulais"
          value={petName}
          onChange={(e) => setPetName(e.target.value)}
          autoComplete="off"
        />
      </div>

      {/* Especie */}
      <div className="flex flex-col gap-1.5">
        <Label htmlFor="booking-species">Especie</Label>
        <Select value={specieId} onValueChange={setSpecieId}>
          <SelectTrigger id="booking-species" className="w-full">
            <SelectValue placeholder="Selecciona la especie" />
          </SelectTrigger>
          <SelectContent>
            {species?.map((s) => (
              <SelectItem key={s.id} value={String(s.id)}>
                {s.speciesName}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      {/* Motivo de consulta */}
      <div className="flex flex-col gap-1.5">
        <Label htmlFor="booking-observation">Motivo de la consulta (opcional)</Label>
        <Textarea
          id="booking-observation"
          placeholder="Ej. Control de rutina, vacunación..."
          value={observation}
          onChange={(e) => setObservation(e.target.value)}
          rows={3}
        />
      </div>

      <Button
        type="submit"
        disabled={!isValid || isSubmitting}
        className="w-full bg-primary text-primary-foreground hover:bg-primary/90"
      >
        {isSubmitting ? "Enviando solicitud..." : "Reservar cita"}
      </Button>
    </form>
  )
}