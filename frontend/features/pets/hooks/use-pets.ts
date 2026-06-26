"use client"

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { petService } from "../api/pet.service"
import type { CreatePetDto, UpdatePetDto } from "../types/pet.types"

export const PETS_KEY = ["pets"] as const

export function usePets(page = 1, pageSize = 10, search = "") {
  return useQuery({
    queryKey: [...PETS_KEY, page, pageSize, search],
    queryFn: () => petService.getAll(page, pageSize, search),
  })
}

export function useCreatePet() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (dto: CreatePetDto) => petService.create(dto),
    onSuccess: () => qc.invalidateQueries({ queryKey: PETS_KEY }),
  })
}

export function useUpdatePet() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: UpdatePetDto }) =>
      petService.update(id, dto),
    onSuccess: () => qc.invalidateQueries({ queryKey: PETS_KEY }),
  })
}

export function useDeletePet() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => petService.delete(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: PETS_KEY }),
  })
}

export function useChangePetState() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, isActive }: { id: number; isActive: boolean }) =>
      petService.changeState(id, isActive),
    onSuccess: () => qc.invalidateQueries({ queryKey: PETS_KEY }),
  })
}