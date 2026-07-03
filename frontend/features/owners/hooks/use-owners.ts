"use client"

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { ownerService } from "../api/owner.service"
import type { CreateOwnerDto, UpdateOwnerDto } from "../types/owner.types"
import { toast } from "sonner"

export const OWNERS_KEY = ["owners"] as const

export function useOwners() {
  return useQuery({
    queryKey: OWNERS_KEY,
    queryFn: () => ownerService.getAll(1, 100, ""),
  })
}

export function useAllOwners() {
  return useQuery({
    queryKey: [...OWNERS_KEY, "all"],
    queryFn: () => ownerService.getAll(1, 100, ""),
  })
}

export function useCreateOwner() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (dto: CreateOwnerDto) => ownerService.create(dto),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: OWNERS_KEY })
      toast.success("Propietario Creado")  
    },
    onError: () => {
      toast.error("No se pudo crear el propietario")
    },
  })
}

export function useUpdateOwner() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: UpdateOwnerDto }) =>
      ownerService.update(id, dto),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: OWNERS_KEY })
      toast.success("Propietario actualizado")
    },
    onError: () => {
      toast.error("No se pudo actualizar el propietario")
    },
  })
}

export function useDeleteOwner() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => ownerService.delete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: OWNERS_KEY })
      toast.success("Propierario Eliminado")
    },
    onError: () => {
      toast.success("El propietario no pudo ser eliminado")
    }
  })
}