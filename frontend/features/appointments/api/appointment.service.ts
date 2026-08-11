import { appointmentRepository } from "./appointment.repository"
import type { CreateAppointmentDto, UpdateAppointmentDto } from "../types/appointment.types"

export const appointmentService = {
  getAll: (page: number, pageSize: number, search: string, petId?: number) => {
    return appointmentRepository.getAll(page, pageSize, search, petId)
  },

  create: (dto: CreateAppointmentDto) => {
    return appointmentRepository.create(dto)
  },
  update: (dto: UpdateAppointmentDto) => {
    return appointmentRepository.update(dto)
  },
  delete: (id: number) => {
    return appointmentRepository.delete(id)
  },
  changeState: (id: number, stateId: number) => {
    return appointmentRepository.changeState(id, stateId)
  },
  getStates: () => {
    return appointmentRepository.getStates()
  },
  getBillable: (ownerId: number) => {
  return appointmentRepository.getBillable(ownerId)
},
}
