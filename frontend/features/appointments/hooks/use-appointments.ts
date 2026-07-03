import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { appointmentService } from "../api/appointment.service"
import type { CreateAppointmentDto, UpdateAppointmentDto } from "../types/appointment.types"
import { toast } from "sonner"

const QUERY_KEY = ["appointments"] as const

export const useAppointments = (page = 1, pageSize = 10, search = "") => {
  return useQuery({
    queryKey: [...QUERY_KEY, page, pageSize, search],
    queryFn: () => appointmentService.getAll(page, pageSize, search),
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
    onError: () => {
      toast.error("No se pudo crear la cita")
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
    onError: () => {
      toast.error("No se pudo actualizar la cita")
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
    onError: () => {
      toast.error("No se pudo eliminar la cita")
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
    onError: () => {
      toast.error("No se pudo actualizar el estado de la cita")
    },
  })
}