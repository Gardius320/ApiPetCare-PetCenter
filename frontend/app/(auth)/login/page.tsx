'use client'
import { useState } from 'react'
import { useAuth } from '@/features/auth/hooks/use-auth'
import { PawPrint } from 'lucide-react'

export default function LoginPage() {
  const { login, isLoading, error } = useAuth()
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    await login({ email, password })
  }

  return (
    <div className="relative min-h-screen overflow-hidden bg-[#F5F7F4] flex items-center justify-center p-4 font-[family-name:var(--font-inter)]">
      {/* Decoracion de fondo */}
      <div
        aria-hidden="true"
        className="pointer-events-none absolute -top-28 -right-24 h-80 w-80 rounded-full bg-[#1F6F5C]/10 blur-3xl"
      />
      <div
        aria-hidden="true"
        className="pointer-events-none absolute -bottom-28 -left-24 h-80 w-80 rounded-full bg-[#E0A458]/15 blur-3xl"
      />
      <PawPrint
        aria-hidden="true"
        className="pointer-events-none absolute top-12 left-12 h-16 w-16 -rotate-12 text-[#1F6F5C]/10"
      />
      <PawPrint
        aria-hidden="true"
        className="pointer-events-none absolute bottom-16 right-16 h-24 w-24 rotate-12 text-[#FF8C6B]/10"
      />

      <div className="relative w-full max-w-md overflow-hidden rounded-2xl bg-white shadow-[0_1px_3px_rgba(0,0,0,0.05),0_20px_40px_-20px_rgba(22,48,43,0.2)]">
        <div className="h-1.5 w-full bg-gradient-to-r from-[#1F6F5C] via-[#E0A458] to-[#FF8C6B]" />

        <div className="p-8">
          <div className="mb-8 flex flex-col items-center">
            <div className="mb-4 flex h-14 w-14 items-center justify-center rounded-2xl bg-[#1F6F5C] shadow-[0_1px_3px_rgba(0,0,0,0.05)]">
              <PawPrint className="h-7 w-7 text-white" />
            </div>
            <h1 className="font-[family-name:var(--font-space-grotesk)] font-medium text-2xl text-[#16302B]">
              PetCare
            </h1>
            <div className="mt-1.5 flex items-center gap-1.5">
              <span className="h-[6px] w-[6px] rounded-full bg-[#1F6F5C]" />
              <p className="text-[11px] font-medium uppercase tracking-wide text-[#1F6F5C]">
                Sistema de gestión veterinaria
              </p>
            </div>
          </div>

          {error && (
            <div className="mb-4 rounded-lg border border-[#B5443A]/20 bg-[#B5443A]/10 px-4 py-3 text-sm text-[#B5443A]">
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="mb-1.5 block text-sm font-medium text-[#16302B]">
                Email
              </label>
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="correo@ejemplo.com"
                required
                className="w-full rounded-lg border border-[#16302B]/15 px-3 py-2.5 text-sm text-[#16302B] placeholder:text-[#5B6B66]/70 transition-colors focus:border-[#1F6F5C] focus:outline-none focus:ring-2 focus:ring-[#1F6F5C]/30"
              />
            </div>
            <div>
              <label className="mb-1.5 block text-sm font-medium text-[#16302B]">
                Contraseña
              </label>
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••"
                required
                className="w-full rounded-lg border border-[#16302B]/15 px-3 py-2.5 text-sm text-[#16302B] placeholder:text-[#5B6B66]/70 transition-colors focus:border-[#1F6F5C] focus:outline-none focus:ring-2 focus:ring-[#1F6F5C]/30"
              />
            </div>
            <button
              type="submit"
              disabled={isLoading}
              className="w-full cursor-pointer rounded-lg bg-[#1F6F5C] py-2.5 text-sm font-medium text-white transition-colors hover:bg-[#18594a] disabled:cursor-not-allowed disabled:bg-[#1F6F5C]/50"
            >
              {isLoading ? 'Iniciando sesión...' : 'Iniciar sesión'}
            </button>
          </form>
        </div>
      </div>
    </div>
  )
}
