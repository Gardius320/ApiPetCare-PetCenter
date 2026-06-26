"use client"

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { speciesService } from "../api/species.service"
import type { CreateSpeciesDto } from "../types/species.types"

export const SPECIES_KEY = ["species"] as const

export function useSpecies() {
  return useQuery({
    queryKey: SPECIES_KEY,
    queryFn: () => speciesService.getAll(),
  })
}

export function useCreateSpecies() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (dto: CreateSpeciesDto) => speciesService.create(dto),
    onSuccess: () => qc.invalidateQueries({ queryKey: SPECIES_KEY }),
  })
}

export function useDeleteSpecies() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => speciesService.delete(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: SPECIES_KEY }),
  })
}