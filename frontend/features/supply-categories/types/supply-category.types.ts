export interface SupplyCategory {
  id: number
  name: string
  description: string | null
  isActive: boolean
}
export interface CreateSupplyCategoryDto {
  name: string
  description: string | null
}

export  interface UpdateSupplyCategoryDto {
  name?: string
  description?: string | null
  
}

export interface DeleteSupplyCategoryDto {
  id: number
}