"use client"

import { useState } from "react"
import { PawPrint, CheckCircle2 } from "lucide-react"
import { DateSlotPicker } from "@/features/booking/components/date-slot-picker"
import { BookingForm } from "@/features/booking/components/booking-form"
import { useBookOnline } from "@/features/booking/hooks/use-booking"
import type { BookOnlineDto } from "@/features/booking/types/booking.types"

export default function ReservarPage() {
  const [date, setDate] = useState("")
  const [slot, setSlot] = useState<string | null>(null)
  const [confirmed, setConfirmed] = useState(false)

  const bookOnline = useBookOnline()

  function handleSubmit(dto: BookOnlineDto) {
    bookOnline.mutate(dto, {
      onSuccess: () => setConfirmed(true),
    })
  }

  function handleReset() {
    setDate("")
    setSlot(null)
    setConfirmed(false)
  }

  return (
    <div className="relative min-h-screen overflow-hidden bg-[#F5F7F4] flex items-center justify-center p-4 font-[family-name:var(--font-inter)]">
      {/* Decoración de fondo — mismo estilo que login */}
      <div
        aria-hidden="true"
        className="pointer-events-none absolute -top-28 -right-24 h-80 w-80 rounded-full bg-[#1F6F5C]/10 blur-3xl"
      />
      <div
        aria-hidden="true"
        className="pointer-events-none absolute -bottom-28 -left-24 h-80 w-80 rounded-full bg-[#E0A458]/15 blur-3xl"
      />

      <div className="relative w-full max-w-md overflow-hidden rounded-2xl bg-white shadow-[0_1px_3px_rgba(0,0,0,0.05),0_20px_40px_-20px_rgba(22,48,43,0.2)]">
        <div className="h-1.5 w-full bg-gradient-to-r from-[#1F6F5C] via-[#E0A458] to-[#FF8C6B]" />

        <div className="p-8">
          <div className="mb-8 flex flex-col items-center">
            <div className="mb-4 flex h-14 w-14 items-center justify-center rounded-2xl bg-[#1F6F5C] shadow-[0_1px_3px_rgba(0,0,0,0.05)]">
              <PawPrint className="h-7 w-7 text-white" />
            </div>
            <h1 className="font-[family-name:var(--font-space-grotesk)] font-medium text-2xl text-[#16302B]">
              Reserva tu cita
            </h1>
            <p className="mt-1.5 text-[11px] font-medium uppercase tracking-wide text-[#1F6F5C]">
              PetCare · Sistema de gestión veterinaria
            </p>
          </div>

          {confirmed ? (
            <div className="flex flex-col items-center gap-3 py-4 text-center">
              <CheckCircle2 className="h-12 w-12 text-[#1F6F5C]" />
              <p className="text-sm text-[#16302B]">
                Solicitud de cita recibida. Te contactaremos para confirmarla.
              </p>
              <button
                type="button"
                onClick={handleReset}
                className="mt-2 text-sm font-medium text-[#1F6F5C] underline underline-offset-2"
              >
                Reservar otra cita
              </button>
            </div>
          ) : (
            <div className="flex flex-col gap-6">
              <DateSlotPicker
                selectedDate={date}
                onDateChange={setDate}
                selectedSlot={slot}
                onSlotChange={setSlot}
              />

              {date && slot && (
                <BookingForm
                  date={date}
                  slot={slot}
                  onSubmit={handleSubmit}
                  isSubmitting={bookOnline.isPending}
                />
              )}
            </div>
          )}
        </div>
      </div>
    </div>
  )
}