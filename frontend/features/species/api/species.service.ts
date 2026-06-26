import { speciesRepository } from "./species.repository"
import type { Species, CreateSpeciesDto } from "../types/species.types"

export const speciesService = {
  getAll: () => speciesRepository.getAll(),
  create: (dto: CreateSpeciesDto) => speciesRepository.create(dto),
  delete: (id: number) => speciesRepository.delete(id)
}