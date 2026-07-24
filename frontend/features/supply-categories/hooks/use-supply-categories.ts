import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { supplyCategoryRepository } from "../api/supply-category.repository"
import { toast } from "sonner"
import type { CreateSupplyCategoryDto } from "../types/supply-category.types"

export const SUPPLY_CATEGORIES_KEY = ["supply-categories"] as const

export function useAllSupplyCategories() {
  return useQuery({
    queryKey: SUPPLY_CATEGORIES_KEY,
    queryFn: () => supplyCategoryRepository.all(),
  })
}

export function useCreateSupplyCategory() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (dto: CreateSupplyCategoryDto) => supplyCategoryRepository.create(dto),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: SUPPLY_CATEGORIES_KEY })
      toast.success("Categoría de suministro creada correctamente")
    },
    onError: () => {
      toast.error("No se pudo crear la categoría de suministro")
    },
  })
}

export function useUpdateSupplyCategory() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (params: { id: number; dto: CreateSupplyCategoryDto }) =>
      supplyCategoryRepository.update(params.id, params.dto),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: SUPPLY_CATEGORIES_KEY })
      toast.success("Categoría de suministro actualizada correctamente")
    },
    onError: () => {
      toast.error("No se pudo actualizar la categoría de suministro")
    },
  })
}

export function useDeleteSupplyCategory() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => supplyCategoryRepository.delete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: SUPPLY_CATEGORIES_KEY })
      toast.success("Categoría de suministro eliminada correctamente")
    },
    onError: () => {
      toast.error("No se pudo eliminar la categoría de suministro")
    },
  })
}