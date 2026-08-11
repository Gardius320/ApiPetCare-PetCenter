"use client"

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { userService } from "../api/user.service"
import type { CreateUserDto, ChangeRoleDto } from "../types/user.types"
import { toast } from "sonner"
import { getErrorMessage } from "@/lib/get-error-message"

export const USERS_KEY = ["users"] as const

export function useUsers() {
  return useQuery({
    queryKey: USERS_KEY,
    queryFn: () => userService.getAll(),
  })
}

export function useUsersByRole(role: string) {
  return useQuery({
    queryKey: [...USERS_KEY, "by-role", role],
    queryFn: () => userService.getByRole(role),
    enabled: !!role,
  })
}

export function useVeterinarians() {
  return useUsersByRole("Veterinarian")
}

export function useCreateUser() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (dto: CreateUserDto) => userService.create(dto),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: USERS_KEY })
      toast.success("Usuario creado")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo crear el usuario"))
    },
  })
}

export function useChangeRole() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, dto }: { id: string; dto: ChangeRoleDto }) =>
      userService.changeRole(id, dto),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: USERS_KEY })
      toast.success("Rol actualizado")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo actualizar el rol"))
    },
  })
}

export function useDeleteUser() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: string) => userService.delete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: USERS_KEY })
      toast.success("Usuario eliminado")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "El usuario no pudo ser eliminado"))
    },
  })
}