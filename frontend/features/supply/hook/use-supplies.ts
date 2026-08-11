import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { supplyService } from '../api/supply.service'
import type { CreateSupplyDto, UpdateSupplyDto } from '../types/supply.types'
import { toast } from "sonner"
import { getErrorMessage } from "@/lib/get-error-message"

export const SUPPLIES_KEY = ['supplies'] as const
export const SUPPLY_STATS_KEY = ['supplies', 'stats'] as const

export function useSupplies(page = 1, pageSize = 10, search = '', categoryId?: number, onlyActive = true) {
  return useQuery({
    queryKey: [...SUPPLIES_KEY, page, pageSize, search, categoryId, onlyActive],
    queryFn: () => supplyService.getAll(page, pageSize, search, categoryId, onlyActive),
  })
}

export function useSupplyStats() {
  return useQuery({
    queryKey: SUPPLY_STATS_KEY,
    queryFn: () => supplyService.getStats(),
  })
}

export function useCreateSupply() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (dto: CreateSupplyDto) => supplyService.create(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: SUPPLIES_KEY })
      queryClient.invalidateQueries({ queryKey: SUPPLY_STATS_KEY })
      toast.success("Insumo creado")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo crear el insumo"))
    }
  })
}

export function useUpdateSupply() {
    const qc = useQueryClient()
    return useMutation({
      mutationFn: ({ id, dto }: { id: number; dto: UpdateSupplyDto }) =>
        supplyService.update(id, dto),
      onSuccess: () => {
        qc.invalidateQueries({ queryKey: SUPPLIES_KEY })
        qc.invalidateQueries({ queryKey: SUPPLY_STATS_KEY })
        toast.success("Insumo actualizado")
      },
      onError: (err) => {
        toast.error(getErrorMessage(err, "No se pudo actualizar el insumo"))
      },
    })
}

export function useToggleSupplyStatus() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => supplyService.toggleStatus(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: SUPPLIES_KEY })
      qc.invalidateQueries({ queryKey: SUPPLY_STATS_KEY })
      toast.success("Estado del insumo actualizado")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo actualizar el estado del insumo"))
    },
  })
}