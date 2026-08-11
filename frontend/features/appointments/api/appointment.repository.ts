import api from "@/lib/axios"
import type { Appointment, AppointmentState, BillableAppointment, CreateAppointmentDto, UpdateAppointmentDto } from "../types/appointment.types"

interface RawAppointment {
  id: number
  date: string
  ownerId: number
  ownerName: string
  petName: string
  state: string
  observation: string
  species: string
}

interface RawState {
  idState: number
  stateName: string
  description: string | null
}

function mapAppointment(a: RawAppointment): Appointment {
  return {
    id: a.id,
    date: a.date,
    ownerId: a.ownerId,
    ownerName: a.ownerName,
    petName: a.petName,
    state: a.state,
    observation: a.observation,
    species: a.species,
  }
}

export const appointmentRepository = {
  getAll: async (page = 1, pageSize = 10, search = "", petId?: number): Promise<{ items: Appointment[]; total: number }>=> {
    const response = await api.get<{ data: { items: RawAppointment[]; totalRecords: number } }>("/Appointment/GetAll", {
      params: { page, pageSize, search, petId }
    })
    return {
      items: response.data.data.items.map(mapAppointment),
      total: response.data.data.totalRecords
    }
  },

  create: async (dto: CreateAppointmentDto): Promise<number | null> => {
    const response = await api.post<{ data: number | null }>("/Appointment/Create", dto)
    return response.data.data
  },

  update: async (dto: UpdateAppointmentDto): Promise<number | null> => {
    const { id, ...body } = dto
    const response = await api.put<{ data: number | null }>(`/Appointment/Update/${id}`, body)
    return response.data.data
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/Appointment/Delete/${id}`)
  },

  changeState: async (id: number, stateId: number): Promise<void> => {
    await api.patch(`/Appointment/ChangeState/${id}`, { stateId })
  },

  getStates: async (): Promise<AppointmentState[]> => {
    const response = await api.get<{ data: RawState[] }>("/States/GetAll")
    return response.data.data.map((s) => ({
      id: s.idState,
      name: s.stateName,
      description: s.description,
    }))
  },
  getBillable: async (ownerId: number): Promise<BillableAppointment[]> => {
  const response = await api.get<{ data: BillableAppointment[] }>("/Appointment/Billable", {
    params: { ownerId }
  })
  return response.data.data
},
}
