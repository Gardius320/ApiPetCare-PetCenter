"use client"

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { speciesService } from "../api/species.service"
import type { CreateSpeciesDto } from "../types/species.types"
import { toast } from "sonner"

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
    onSuccess: () => { 
      qc.invalidateQueries({ queryKey: SPECIES_KEY })
      toast.success("Especie creada")
    },
    onError: () => {
       toast.error("La especie no pudo ser creda")
    }
  })
}

export function useDeleteSpecies() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => speciesService.delete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: SPECIES_KEY })
      toast.success("La especie fue eliminada")
    },
    onError: () => {
      toast.error("La especie no pudo ser eliminada")
    }
  })
}