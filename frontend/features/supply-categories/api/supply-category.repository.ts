import api from "@/lib/axios"
import type { ApiResponse } from "@/lib/types/api-response.types"
import type { CreateSupplyCategoryDto, PaginatedSupplyCategories, SupplyCategory } from "../types/supply-category.types"

const BASE = "/SupplyCategories"

export interface GetAllSupplyCategoriesParams {
  page?: number
  pageSize?: number
  search?: string
  onlyActive?: boolean
}

export const supplyCategoryRepository = {
  getAll(params: GetAllSupplyCategoriesParams = {}): Promise<PaginatedSupplyCategories> {
    return api
      .get<ApiResponse<PaginatedSupplyCategories>>(`${BASE}/GetAll`, { params })
      .then((res) => res.data.data ?? { items: [], totalRecords: 0, totalPages: 1 })
  },

  create(supplyCategory: CreateSupplyCategoryDto): Promise<number> {
    return api.post<number>(`${BASE}/Create`, supplyCategory).then((res) => res.data)
  },

  update(id: number, supplyCategory: CreateSupplyCategoryDto): Promise<ApiResponse<SupplyCategory>> {
    return api.put<ApiResponse<SupplyCategory>>(`${BASE}/Update/${id}`, supplyCategory).then((res) => res.data)
  },

  delete(id: number): Promise<ApiResponse<string>> {
    return api.delete<ApiResponse<string>>(`${BASE}/Delete/${id}`).then((res) => res.data)
  },
}