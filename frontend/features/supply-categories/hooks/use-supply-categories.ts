import { useQuery } from "@tanstack/react-query"
import { supplyCategoryRepository } from "../api/supply-category.repository"

export const SUPPLY_CATEGORIES_KEY = ["supply-categories"] as const

export function useAllSupplyCategories() {
  return useQuery({
    queryKey: SUPPLY_CATEGORIES_KEY,
    queryFn: () => supplyCategoryRepository.getAll(),
  })
}