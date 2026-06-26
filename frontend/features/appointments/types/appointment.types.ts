export interface Appointment {
  id: number
  fecha: string
  nombreDueno: string
  nombreMascota: string
  estado: string
  observacion: string
  especie: string
}

export interface CreateAppointmentDto {
  ownerId: number
  petId: number
  appointmentDate: string
  observation: string
}

export interface UpdateAppointmentDto {
  appointmentDate?: string
  observation?: string
  id: number
}