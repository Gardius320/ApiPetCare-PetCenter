import axios from 'axios'
import type { LoginDto, AuthResponse } from '../types/auth.types'

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
  }
}