export interface Owner {
  id: number
  ownerName: string
  email: string
  phone: string
  gender: string
  estado: string
}

export interface CreateOwnerDto {
  ownerName: string
  email: string
  phone: string
  gender: string
}

export interface UpdateOwnerDto {
  ownerName: string
  email: string
  phone: string
  gender: string
}

export interface OwnerStats {
  total: number
  hombres: number
  mujeres: number
  inactivos: number
}
