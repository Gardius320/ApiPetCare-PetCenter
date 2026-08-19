export interface Supplier {
  id: number
  name: string
  contactNumber: string | null
  email: string | null
  address: string | null
  description: string | null
  isActive: boolean
}

export interface CreateSupplierDto {
  name: string
  contactNumber?: string | null
  email?: string | null
  address?: string | null
  description?: string | null
}

export interface UpdateSupplierDto {
  name?: string
  contactNumber?: string | null
  email?: string | null
  address?: string | null
  description?: string | null
}

export interface PaginatedSuppliers {
  items: Supplier[]
  totalRecords: number
  totalPages: number
}