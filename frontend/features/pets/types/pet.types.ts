export interface Pet {
  id: number
  name: string
  species: string
  ownerId: number
  ownerName: string
  emailOwner: string
  isActive: boolean
}

export interface CreatePetDto {
  petName: string
  specieId: number
  ownerId: number
}

export type UpdatePetDto = Partial<CreatePetDto>

export interface PetStats {
  total: number
  active: number
  inactive: number
}
