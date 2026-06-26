import { userRepository } from "./user.repository"
import type { CreateUserDto, ChangeRoleDto } from "../types/user.types"

export const userService = {
  getAll: () => userRepository.getAll(),
  create: (dto: CreateUserDto) => userRepository.create(dto),
  changeRole: (id: string, dto: ChangeRoleDto) => userRepository.changeRole(id, dto),
  delete: (id: string) => userRepository.delete(id)
}