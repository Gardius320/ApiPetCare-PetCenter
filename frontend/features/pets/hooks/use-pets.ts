"use client"

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { toast } from "sonner"
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
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: PETS_KEY })
      toast.success("Mascota creada correctamente")
    },
    onError: () => {
      toast.error("No se pudo crear la mascota")
    },
  })
}

export function useUpdatePet() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: UpdatePetDto }) =>
      petService.update(id, dto),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: PETS_KEY })
      toast.success("Mascota actualizada correctamente")
    },
    onError: () => {
      toast.error("No se pudo actualizar la mascota")
    },
  })
}

export function useChangePetState() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, isActive }: { id: number; isActive: boolean }) =>
      petService.changeState(id, isActive),
    onSuccess: (_, variables) => {
      qc.invalidateQueries({ queryKey: PETS_KEY })
      toast.success(variables.isActive ? "Mascota activada" : "Mascota desactivada")
    },
    onError: () => {
      toast.error("No se pudo cambiar el estado de la mascota")
    },
  })
}