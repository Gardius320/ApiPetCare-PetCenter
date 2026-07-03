'use client'
import axios from 'axios'
import type { AuthResponse } from '@/features/auth/types/auth.types'

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  },
  withCredentials: false,
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

let isRefreshing = false
let pendingRequests: Array<(token: string) => void> = []

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config

    if (error.response?.status !== 401 || originalRequest._retry) {
      return Promise.reject(error)
    }

    const refreshToken = localStorage.getItem('refreshToken')
    if (!refreshToken) {
      redirectToLogin()
      return Promise.reject(error)
    }

    originalRequest._retry = true

    if (isRefreshing) {
      return new Promise((resolve) => {
        pendingRequests.push((token) => {
          originalRequest.headers.Authorization = `Bearer ${token}`
          resolve(api(originalRequest))
        })
      })
    }

    isRefreshing = true

    try {
      const res = await axios.post<AuthResponse>(
        `${process.env.NEXT_PUBLIC_API_URL}/Auth/refresh`,
        { refreshToken },
        { headers: { 'Content-Type': 'application/json' } }
      )

      const { token, refreshToken: newRefreshToken } = res.data

      localStorage.setItem('token', token)
      localStorage.setItem('refreshToken', newRefreshToken)

      pendingRequests.forEach((cb) => cb(token))
      pendingRequests = []

      originalRequest.headers.Authorization = `Bearer ${token}`
      return api(originalRequest)
    } catch {
      localStorage.clear()
      redirectToLogin()
      return Promise.reject(error)
    } finally {
      isRefreshing = false
    }
  }
)

function redirectToLogin() {
  if (typeof window !== 'undefined') {
    window.location.href = '/login'
  }
}

export default api
