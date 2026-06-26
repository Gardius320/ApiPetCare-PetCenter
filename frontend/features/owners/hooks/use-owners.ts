"use client"

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { ownerService } from "../api/owner.service"
import type { CreateOwnerDto, UpdateOwnerDto } from "../types/owner.types"

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
    onSuccess: () => qc.invalidateQueries({ queryKey: OWNERS_KEY }),
  })
}

export function useUpdateOwner() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: UpdateOwnerDto }) =>
      ownerService.update(id, dto),
    onSuccess: () => qc.invalidateQueries({ queryKey: OWNERS_KEY }),
  })
}

export function useDeleteOwner() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => ownerService.delete(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: OWNERS_KEY }),
  })
}