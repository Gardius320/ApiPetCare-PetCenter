import axios from 'axios'
import type { LoginDto, AuthResponse, RefreshTokenDto } from '../types/auth.types'

const authApi = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  },
})

export const authRepository = {
  login: async (dto: LoginDto): Promise<AuthResponse> => {
    const res = await authApi.post<AuthResponse>('/Auth/login', dto)
    return res.data
  },

  refresh: async (dto: RefreshTokenDto): Promise<AuthResponse> => {
    const res = await authApi.post<AuthResponse>('/Auth/refresh', dto)
    return res.data
  }
}
