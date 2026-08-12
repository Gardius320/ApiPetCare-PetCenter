"use client"

import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Button } from "@/components/ui/button"
import { useAvailableSlots } from "../hooks/use-booking"

interface Props {
  selectedDate: string
  onDateChange: (date: string) => void
  selectedSlot: string | null
  onSlotChange: (slot: string) => void
}

export function DateSlotPicker({
  selectedDate,
  onDateChange,
  selectedSlot,
  onSlotChange,
}: Props) {
  const today = new Date().toISOString().slice(0, 10)

  const { data: slots, isLoading } = useAvailableSlots(selectedDate)

  return (
    <div className="flex flex-col gap-4">
      {/* Selector de fecha */}
      <div className="flex flex-col gap-1.5">
        <Label htmlFor="booking-date">Fecha</Label>
        <Input
          id="booking-date"
          type="date"
          min={today}
          value={selectedDate}
          onChange={(e) => {
            onDateChange(e.target.value)
            onSlotChange("") // limpia la hora elegida si cambia la fecha
          }}
        />
      </div>

      {/* Grilla de horarios */}
      {selectedDate && (
        <div className="flex flex-col gap-1.5">
          <Label>Horario disponible</Label>

          {isLoading && (
            <p className="text-sm text-[#5B6B66]">Cargando horarios...</p>
          )}

          {!isLoading && slots && slots.length === 0 && (
            <p className="text-sm text-[#5B6B66]">
              No hay horarios disponibles ese día. Intenta con otra fecha.
            </p>
          )}

          {!isLoading && slots && slots.length > 0 && (
            <div className="grid grid-cols-3 gap-2 sm:grid-cols-4">
              {slots.map((hora) => (
                <Button
                  key={hora}
                  type="button"
                  variant={hora === selectedSlot ? "default" : "outline"}
                  className={
                    hora === selectedSlot
                      ? "bg-[#1F6F5C] text-white hover:bg-[#18594a]"
                      : ""
                  }
                  onClick={() => onSlotChange(hora)}
                >
                  {hora}
                </Button>
              ))}
            </div>
          )}
        </div>
      )}
    </div>
  )
}