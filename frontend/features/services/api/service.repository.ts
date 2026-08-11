import api from "@/lib/axios"
import type {Service, CreateServiceDto, UpdateServiceDto} from "../types/service.types"

interface RawService {
  id: number
  name: string
  description: string | null
  price: number
  isActive: boolean
}

interface ApiResponse<T> {
  isSuccess: boolean
  message: string
  data: T | null
  errors: string[]
}

function mapService(s: RawService): Service {
  return {
    id: s.id,
    name: s.name,
    description: s.description,
    price: s.price,
    isActive: s.isActive,
  }
}

export const serviceRepository = {
  getAll: async (page = 1, pageSize = 10, search = ""): Promise<Service[]> => {
    const response = await api.get<RawService[]>("/Services/all", {
        params: { page, pageSize, search }
    })
    return response.data.map(mapService)
  },

  getById: async (id: number): Promise<Service | null> => {
    const response = await api.get<ApiResponse<RawService>>(`/Services/${id}`)
    if (!response.data.isSuccess || !response.data.data) return null
    return mapService(response.data.data)
  },

  create: async (dto: CreateServiceDto): Promise<number | null> => {
    const response = await api.post<number | null>("/Services/Create", dto)
    return response.data
  },

  update: async (dto: UpdateServiceDto): Promise<Service | null> => {
    const { id, ...body } = dto
    const response = await api.put<ApiResponse<RawService>>(`/Services/Update/${id}`, body)
    return response.data.data ? mapService(response.data.data) : null
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/Services/Delete/${id}`)
  }
}
