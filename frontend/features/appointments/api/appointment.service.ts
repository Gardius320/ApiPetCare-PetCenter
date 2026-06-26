import { appointmentRepository } from "./appointment.repository"
import type { CreateAppointmentDto, UpdateAppointmentDto } from "../types/appointment.types"

export const appointmentService = {
  getAll: (page: number, pageSize: number, search: string) => {
    return appointmentRepository.getAll(page, pageSize, search)
  },

  create: (dto: CreateAppointmentDto) => {
    return appointmentRepository.create(dto)
  },
  update: (dto: UpdateAppointmentDto) => {
    return appointmentRepository.update(dto)
  },
  delete: (id: number) => {
    return appointmentRepository.delete(id)
  }
}