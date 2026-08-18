import Link from "next/link"
import { PawPrint, CalendarPlus, Lock } from "lucide-react"

export default function RootPage() {
  return (
    <div className="relative min-h-screen overflow-hidden bg-muted flex items-center justify-center p-4">
      <div
        aria-hidden="true"
        className="pointer-events-none absolute -top-28 -right-24 h-80 w-80 rounded-full bg-primary/10 blur-3xl"
      />
      <div
        aria-hidden="true"
        className="pointer-events-none absolute -bottom-28 -left-24 h-80 w-80 rounded-full bg-chart-3/15 blur-3xl"
      />

      <div className="relative flex flex-col items-center gap-8 w-full max-w-lg">
        <div className="flex flex-col items-center gap-2">
          <div className="flex h-14 w-14 items-center justify-center rounded-2xl bg-primary shadow-[0_1px_3px_rgba(0,0,0,0.05)]">
            <PawPrint className="h-7 w-7 text-primary-foreground" />
          </div>
          <h1 className="font-heading font-medium text-2xl text-foreground">PetCare</h1>
          <p className="text-[11px] font-medium uppercase tracking-wide text-primary">
            Sistema de gestión veterinaria
          </p>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 w-full">
          <Link
            href="/reservar"
            className="flex flex-col items-center text-center gap-3 rounded-xl border border-border bg-card p-6 hover:border-primary transition-colors"
          >
            <div className="flex h-11 w-11 items-center justify-center rounded-lg bg-primary/10">
              <CalendarPlus className="h-5 w-5 text-primary" />
            </div>
            <div>
              <p className="font-medium text-sm text-foreground">Soy cliente</p>
              <p className="mt-1 text-xs text-muted-foreground">Reserva una cita para tu mascota</p>
            </div>
            <span className="mt-1 w-full rounded-lg bg-primary py-2 text-xs font-medium text-primary-foreground">
              Reservar cita
            </span>
          </Link>

          <Link
            href="/login"
            className="flex flex-col items-center text-center gap-3 rounded-xl border border-border bg-card p-6 hover:border-primary transition-colors"
          >
            <div className="flex h-11 w-11 items-center justify-center rounded-lg bg-chart-3/15">
              <Lock className="h-5 w-5 text-chart-3" />
            </div>
            <div>
              <p className="font-medium text-sm text-foreground">Soy del personal</p>
              <p className="mt-1 text-xs text-muted-foreground">Ingresa al panel de administración</p>
            </div>
            <span className="mt-1 w-full rounded-lg border border-border py-2 text-xs font-medium text-foreground">
              Iniciar sesión
            </span>
          </Link>
        </div>
      </div>
    </div>
  )
}