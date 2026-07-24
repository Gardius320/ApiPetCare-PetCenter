export const GENDER = {
  MALE: "Masculino",
  FEMALE: "Femenino",
} as const

export type Gender = typeof GENDER[keyof typeof GENDER]

export interface Owner {
  id: number
  ownerName: string
  email: string
  phoneNumber: string
  idCard: string
  gender: string
}

export interface CreateOwnerDto {
  ownerName: string
  email: string
  phoneNumber: string
  idCard: string
  gender: string
}

export interface UpdateOwnerDto {
  ownerName: string
  ownerEmail: string
  ownerPhone: string
  gender: string
  idCard: string
}

export interface OwnerStats {
  total: number
  male: number
  female: number
}
