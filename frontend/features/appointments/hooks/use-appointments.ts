import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { appointmentService } from "../api/appointment.service"
import type { CreateAppointmentDto, UpdateAppointmentDto } from "../types/appointment.types"
import { toast } from "sonner"
import { getErrorMessage } from "@/lib/get-error-message"

const QUERY_KEY = ["appointments"] as const
const STATES_KEY = ["appointment-states"] as const

export const useAppointments = (page = 1, pageSize = 10, search = "") => {
  return useQuery({
    queryKey: [...QUERY_KEY, page, pageSize, search],
    queryFn: () => appointmentService.getAll(page, pageSize, search),
  })
}

export const useAppointmentsByPetId = (petId: number) => {
  return useQuery({
    queryKey: [...QUERY_KEY, "by-pet", petId],
    queryFn: () => appointmentService.getAll(1, 100, "", petId),
    enabled: !!petId,
  })
}

export const useBillableAppointments = (ownerId: number) => {
  return useQuery({
    queryKey: [...QUERY_KEY, "billable", ownerId],
    queryFn: () => appointmentService.getBillable(ownerId),
    enabled: !!ownerId,
  })
}

export const useAppointmentStates = () => {
  return useQuery({
    queryKey: STATES_KEY,
    queryFn: () => appointmentService.getStates(),
    staleTime: 5 * 60 * 1000,
  })
}

export const useCreateAppointment = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (dto: CreateAppointmentDto) => appointmentService.create(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: QUERY_KEY })
      toast.success("Cita creada correctamente")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo crear la cita"))
    },
  })
}

export const useUpdateAppointment = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (dto: UpdateAppointmentDto) => appointmentService.update(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: QUERY_KEY })
      toast.success("Cita actualizada correctamente")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo actualizar la cita"))
    },
  })
}

export const useDeleteAppointment = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (id: number) => appointmentService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: QUERY_KEY })
      toast.success("Cita eliminada correctamente")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo eliminar la cita"))
    },
  })
}

export const useChangeAppointmentState = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ({ id, stateId }: { id: number; stateId: number }) =>
      appointmentService.changeState(id, stateId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: QUERY_KEY })
      toast.success("Estado de la cita actualizado")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo actualizar el estado de la cita"))
    },
  })
}