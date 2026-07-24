import api from "@/lib/axios"
import type { ApiResponse } from "@/lib/types/api-response.types"
import type { CreateSupplyCategoryDto, SupplyCategory } from "../types/supply-category.types"

const BASE = "/SupplyCategories"

export const supplyCategoryRepository = {
  all(): Promise<SupplyCategory[]> {
    return api.get<SupplyCategory[]>(`${BASE}/all`).then((res) => res.data)
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