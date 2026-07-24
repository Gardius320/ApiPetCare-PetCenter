import { petRepository } from "./pet.repository"
import type { Pet, CreatePetDto, UpdatePetDto, PetStats } from "../types/pet.types"

export const petService = {
  getAll: (page: number, pageSize: number, search: string) =>
    petRepository.getAll(page, pageSize, search),

  create: (dto: CreatePetDto): Promise<Pet> => petRepository.create(dto),

  update: (id: number, dto: UpdatePetDto): Promise<Pet> =>
    petRepository.update(id, dto),

  changeState: (id: number, isActive: boolean): Promise<void> =>
    petRepository.changeState(id, isActive),

  computeStats(pets: Pet[]): PetStats {
    return {
      total: pets.length,
      active: pets.filter((p) => p.isActive).length,
      inactive: pets.filter((p) => !p.isActive).length,
    }
  },
}
