"use client"

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { userService } from "../api/user.service"
import type { CreateUserDto, ChangeRoleDto } from "../types/user.types"

export const USERS_KEY = ["users"] as const

export function useUsers() {
  return useQuery({
    queryKey: USERS_KEY,
    queryFn: () => userService.getAll(),
  })
}

export function useCreateUser() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (dto: CreateUserDto) => userService.create(dto),
    onSuccess: () => qc.invalidateQueries({ queryKey: USERS_KEY }),
  })
}

export function useChangeRole() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, dto }: { id: string; dto: ChangeRoleDto }) =>
      userService.changeRole(id, dto),
    onSuccess: () => qc.invalidateQueries({ queryKey: USERS_KEY }),
  })
}

export function useDeleteUser() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: string) => userService.delete(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: USERS_KEY }),
  })
}