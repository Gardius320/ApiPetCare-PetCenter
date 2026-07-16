import api from "@/lib/axios"
import type { SupplyCategory } from "../types/supply-category.types"

const BASE = "/SupplyCategories"

export const supplyCategoryRepository = {
  getAll(): Promise<SupplyCategory[]> {
    return api.get<SupplyCategory[]>(`${BASE}/all`).then((res) => res.data)
  },
}