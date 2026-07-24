import api from "@/lib/axios"
import type { Species, CreateSpeciesDto } from "../types/species.types"

const BASE = "/Species"

export const speciesRepository = {
  getAll(): Promise<Species[]> {
    return api.get(`${BASE}/GetAll`).then((res) => res.data.data)
  },

  create(dto: CreateSpeciesDto): Promise<void> {
    return api.post(`${BASE}/Create`, dto).then(() => undefined)
  },

  delete(id: number): Promise<void> {
    return api.delete(`${BASE}/Delete/${id}`).then(() => undefined)
  }
}