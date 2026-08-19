import api from "@/lib/axios"
import type { ApiResponse } from "@/lib/types/api-response.types"
import type { CreateSupplierDto, PaginatedSuppliers, Supplier } from "../types/supplier.types"

const BASE = "/Suppliers"

export interface GetAllSuppliersParams {
  page?: number
  pageSize?: number
  search?: string
  onlyActive?: boolean
}

export const supplierRepository = {
  getAll(params: GetAllSuppliersParams = {}): Promise<PaginatedSuppliers> {
    return api
      .get<ApiResponse<PaginatedSuppliers>>(`${BASE}/GetAll`, { params })
      .then((res) => res.data.data ?? { items: [], totalRecords: 0, totalPages: 1 })
  },

  create(supplier: CreateSupplierDto): Promise<number> {
    return api.post<number>(`${BASE}/Create`, supplier).then((res) => res.data)
  },

  update(id: number, supplier: CreateSupplierDto): Promise<ApiResponse<Supplier>> {
    return api.put<ApiResponse<Supplier>>(`${BASE}/Update/${id}`, supplier).then((res) => res.data)
  },

  delete(id: number): Promise<ApiResponse<string>> {
    return api.delete<ApiResponse<string>>(`${BASE}/Delete/${id}`).then((res) => res.data)
  },
}