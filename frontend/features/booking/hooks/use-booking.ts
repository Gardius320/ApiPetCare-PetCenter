"use client"

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { bookingRepository } from "../api/booking.repository"
import type { BookOnlineDto } from "../types/booking.types"
import { toast } from "sonner"
import { getErrorMessage } from "@/lib/get-error-message"

export const AVAILABLE_SLOTS_KEY = ["available-slots"] as const

export function useAvailableSlots(date: string) {
  return useQuery({
    queryKey: [...AVAILABLE_SLOTS_KEY, date],
    queryFn: () => bookingRepository.getAvailableSlots(date),
    enabled: !!date,
  })
}

export function useBookOnline() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (dto: BookOnlineDto) => bookingRepository.bookOnline(dto),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: AVAILABLE_SLOTS_KEY })
      toast.success("Solicitud de cita recibida. Te contactaremos para confirmarla.")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo completar la reserva"))
    },
  })
}