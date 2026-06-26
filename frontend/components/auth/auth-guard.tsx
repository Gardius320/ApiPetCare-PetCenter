'use client'
import { useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'

export default function AuthGuard({ children }: { children: React.ReactNode }) {
  const router = useRouter()
  const [isAuthorized, setIsAuthorized] = useState(false)

  useEffect(() => {
    const token = localStorage.getItem('token')

    if (!token) {
      router.push('/login')
      return
    }

    // Pequeño delay para evitar el setState síncrono
    const timer = setTimeout(() => {
      setIsAuthorized(true)
    }, 0)

    return () => clearTimeout(timer)
  }, [router])

  // Mientras verifica muestra pantalla en blanco
  if (!isAuthorized) return null

  return <>{children}</>
}