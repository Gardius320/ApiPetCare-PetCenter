export interface Appointment {
  id: number
  date: string
  ownerName: string
  petName: string
  state: string
  observation: string
  species: string
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

export interface AppointmentState {
  id: number
  name: string
  description: string | null
}
