 import api from "@/lib/axios"
import type { Appointment, CreateAppointmentDto, UpdateAppointmentDto } from "../types/appointment.types"

interface AppointmentResponse {
  items: Appointment[]
  totalRecords: number
}

export const appointmentRepository = {
  getAll: async (page = 1, pageSize = 10, search = ""): Promise<{ items: Appointment[]; total: number }> => {
    const response = await api.get<{ data: AppointmentResponse }>("/Appointment/GetAll", {
      params: { page, pageSize, search }
    })
    return {
      items: response.data.data.items,
      total: response.data.data.totalRecords
    }
  },

  create: async (dto: CreateAppointmentDto): Promise<Appointment> => {
    const response = await api.post<{ data: Appointment }>("/Appointment/Crear", dto)
    return response.data.data
  },

  update: async (dto: UpdateAppointmentDto): Promise<Appointment> => {
  const { id, ...body } = dto
  const response = await api.put<{ data: Appointment }>(`/Appointment/Actualizar/${id}`, body)
  return response.data.data
},

  delete: async (id: number): Promise<void> => {
    await api.delete(`/Appointment/Eliminar/${id}`)
  }
}