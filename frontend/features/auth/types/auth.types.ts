export interface LoginDto {
  email: string
  password: string
}

export interface AuthResponse {
  token: string
  email: string
  fullName: string
  role: string
  expiration: string
}

export interface ApiResponse<T> {
  isSuccess: boolean
  message: string
  data: T
  errors: string[] | null
}