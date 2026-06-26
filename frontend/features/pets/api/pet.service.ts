import { petRepository } from "./pet.repository"
import type { Pet, CreatePetDto, UpdatePetDto, PetStats } from "../types/pet.types"

export const petService = {
  getAll: (page: number, pageSize: number, search: string) =>
    petRepository.getAll(page, pageSize, search),

  create: (dto: CreatePetDto): Promise<Pet> => petRepository.create(dto),

  update: (id: number, dto: UpdatePetDto): Promise<Pet> =>
    petRepository.update(id, dto),

  delete: (id: number): Promise<void> => petRepository.delete(id),

  changeState: (id: number, isActive: boolean): Promise<void> =>
    petRepository.changeState(id, isActive),

  computeStats(pets: Pet[]): PetStats {
    return {
      total: pets.length,
      activos: pets.filter((p) => p.estadoId === 1).length,
      enTratamiento: pets.filter((p) => p.estadoId === 2).length,
      inactivos: pets.filter((p) => p.estadoId === 3).length,
    }
  },
}