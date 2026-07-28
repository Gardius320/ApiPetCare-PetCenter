'use client'
import { useState } from 'react'
import { useRouter } from 'next/navigation'
import axios from 'axios'
import { authRepository } from '../api/auth.repository'
import type { LoginDto } from '../types/auth.types'

export function useAuth() {
  const router = useRouter()
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const login = async (dto: LoginDto) => {
    setIsLoading(true)
    setError(null)

    try {
      const response = await authRepository.login(dto)

      if (!response.token || !response.role || !response.email || !response.fullName) {
        throw new Error("Respuesta del servidor incompleta")
      }

      localStorage.setItem('token', response.token)
      localStorage.setItem('refreshToken', response.refreshToken)
      localStorage.setItem('role', response.role)
      localStorage.setItem('fullName', response.fullName)
      localStorage.setItem('email', response.email)

      router.push('/dashboard')

    } catch (err) {
      console.error('Error en login:', err)

      if (axios.isAxiosError(err)) {
        if (!err.response) {
          setError('No se pudo conectar con el servidor. Intenta más tarde.')
        } else if (err.response.status === 401) {
          setError('Credenciales incorrectas. Intenta de nuevo.')
        } else {
          setError('Ocurrió un error en el servidor. Intenta más tarde.')
        }
      } else {
        setError('Ocurrió un error inesperado. Intenta de nuevo.')
      }
    } finally {
      setIsLoading(false)
    }
  }

  const logout = async () => {
    const refreshToken = localStorage.getItem('refreshToken')

    try {
      if (refreshToken) {
        await authRepository.logout({ refreshToken })
      }
    } catch (err) {
      console.error('No se pudo revocar el token en backend:', err)
      
    }

    localStorage.removeItem('token')
    localStorage.removeItem('refreshToken')
    localStorage.removeItem('role')
    localStorage.removeItem('fullName')
    localStorage.removeItem('email')

    router.push('/login')
  }

  return { login, logout, isLoading, error }
}