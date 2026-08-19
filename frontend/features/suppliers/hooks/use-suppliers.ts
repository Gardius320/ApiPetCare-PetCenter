import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { supplierRepository, type GetAllSuppliersParams } from "../api/supplier.repository"
import { toast } from "sonner"
import type { CreateSupplierDto } from "../types/supplier.types"

export const SUPPLIERS_KEY = ["suppliers"] as const

export function useAllSuppliers(params: GetAllSuppliersParams = {}) {
  return useQuery({
    queryKey: [...SUPPLIERS_KEY, params],
    queryFn: () => supplierRepository.getAll(params),
    select: (result) => result.items,
  })
}

export function useCreateSupplier() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (dto: CreateSupplierDto) => supplierRepository.create(dto),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: SUPPLIERS_KEY })
      toast.success("Proveedor creado correctamente")
    },
    onError: () => {
      toast.error("No se pudo crear el proveedor")
    },
  })
}

export function useUpdateSupplier() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (params: { id: number; dto: CreateSupplierDto }) =>
      supplierRepository.update(params.id, params.dto),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: SUPPLIERS_KEY })
      toast.success("Proveedor actualizado correctamente")
    },
    onError: () => {
      toast.error("No se pudo actualizar el proveedor")
    },
  })
}

export function useDeleteSupplier() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => supplierRepository.delete(id),
    onSuccess: (data) => {
      if (data.isSuccess) {
        qc.invalidateQueries({ queryKey: SUPPLIERS_KEY })
        toast.success(data.message)
      } else {
        toast.error(data.message)
      }
    },
    onError: () => {
      toast.error("No se pudo eliminar el proveedor")
    },
  })
}