export interface Supply {
  id: number
  name: string
  description: string | null
  unit: string
  currentStock: number
  minimumStock: number
  isActive: boolean
  supplyCategoryId: number
  categoryName: string
  supplyType: number
}

export interface CreateSupplyDto {
  name: string
  description?: string
  unit: string
  currentStock: number
  minimumStock: number
  supplyCategoryId: number
  supplyType: number
}

export interface UpdateSupplyDto {
  name: string
  description?: string
  unit: string
  currentStock: number
  minimumStock: number
  isActive: boolean
  supplyCategoryId: number
  supplyType: number
}

export interface SupplyStats {
  total: number
  lowStock: number
  outOfStock: number
  categoriesInUse: number
}
