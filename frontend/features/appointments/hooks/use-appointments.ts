import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { appointmentService } from "../api/appointment.service"
import type { CreateAppointmentDto, UpdateAppointmentDto } from "../types/appointment.types"

const QUERY_KEY = "appointments"

export const useAppointments = (page = 1, pageSize = 10, search = "") => {
  return useQuery({
    queryKey: [QUERY_KEY, page, pageSize, search],
    queryFn: () => appointmentService.getAll(page, pageSize, search)
  })
}

export const useCreateAppointment = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (dto: CreateAppointmentDto) => appointmentService.create(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [QUERY_KEY] })
    }
  })
}

  export const useUpdateAppointment = () => {
    const queryClient = useQueryClient()

    return useMutation({
      mutationFn: (dto: UpdateAppointmentDto) => appointmentService.update(dto),
      onSuccess: () => {
        queryClient.invalidateQueries({ queryKey: [QUERY_KEY] })
      }
    })
  }

  export const useDeleteAppointment = () => {
    const queryClient = useQueryClient()

    return useMutation({
      mutationFn: (id: number) => appointmentService.delete(id),
      onSuccess: () => {
        queryClient.invalidateQueries({ queryKey: [QUERY_KEY] })
      }
    })
  }