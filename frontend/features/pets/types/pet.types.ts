export interface Pet {
  id: number
  nombre: string
  especieId: number
  especie: string
  propietarioId: number
  propietario: string
  emailPropietario: string
  estadoId: number
  estado: string
}

export interface CreatePetDto {
  nombre: string
  especieId: number
  propietarioId: number
}

export type UpdatePetDto = Partial<CreatePetDto>

export interface PetStats {
  total: number
  activos: number
  enTratamiento: number
  inactivos: number
}
