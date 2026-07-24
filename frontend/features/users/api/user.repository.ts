import api from "@/lib/axios"
import type { User, CreateUserDto, ChangeRoleDto } from "../types/user.types"

const BASE = "/Users"

export const userRepository = {
  getAll(): Promise<User[]> {
    return api.get(`${BASE}/GetAll`).then((res) => res.data.data)
  },

  create(dto: CreateUserDto): Promise<void> {
    return api.post(`${BASE}/Create`, dto).then(() => undefined)
  },

  changeRole(id: string, dto: ChangeRoleDto): Promise<void> {
    return api.put(`${BASE}/ChangeRole/${id}`, dto).then(() => undefined)
  },

  delete(id: string): Promise<void> {
    return api.delete(`${BASE}/Delete/${id}`).then(() => undefined)
  }
}