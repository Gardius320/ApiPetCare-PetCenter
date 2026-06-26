import api from "@/lib/axios"
import type { User, CreateUserDto, ChangeRoleDto } from "../types/user.types"

const BASE = "/Users"

export const userRepository = {
  getAll(): Promise<User[]> {
    return api.get(`${BASE}/GetAll`).then((res) => res.data.data)
  },

  create(dto: CreateUserDto): Promise<void> {
    return api.post(`${BASE}/Crear`, dto).then(() => undefined)
  },

  changeRole(id: string, dto: ChangeRoleDto): Promise<void> {
    return api.put(`${BASE}/CambiarRol/${id}`, dto).then(() => undefined)
  },

  delete(id: string): Promise<void> {
    return api.delete(`${BASE}/Eliminar/${id}`).then(() => undefined)
  }
}