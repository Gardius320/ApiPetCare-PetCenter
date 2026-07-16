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
}

export interface CreateSupplyDto {
  name: string
  description?: string
  unit: string
  currentStock: number
  minimumStock: number
  supplyCategoryId: number
}

export interface UpdateSupplyDto {
  name: string
  description?: string
  unit: string
  currentStock: number
  minimumStock: number
  isActive: boolean
  supplyCategoryId: number
}

export interface SupplyStats {
  total: number
  bajoStock: number
  agotados: number
  categoriasEnUso: number
}