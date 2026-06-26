export interface User {
  id: string
  email: string
  fullName: string
  role: string
}

export interface CreateUserDto {
  email: string
  password: string
  firstName: string
  lastName: string
  role: string
}

export interface ChangeRoleDto {
  role: string
}