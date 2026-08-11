export interface Service {
  id: number
  name: string
  description: string | null
  price: number
  isActive: boolean
} 

export interface CreateServiceDto {
  name: string
  description: string | null
  price: number
}

export interface UpdateServiceDto {
  name?: string
  description?: string | null
  price?: number
  id: number
}

export interface DeleteServiceDto {
  id: number
}