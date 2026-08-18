"use client"

import { useState } from "react"
import { CalendarDays, Plus, Pencil } from "lucide-react"
import { AppointmentFormModal } from "./appointment-form-modal"
import { useAppointments, useAppointmentStates, useChangeAppointmentState } from "../hooks/use-appointments"
import type { Appointment } from "../types/appointment.types"
import { TableSkeleton } from "@/components/shared/table-skeleton"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"

const PAGE_SIZE = 10

const STATE_COLORS: Record<string, string> = {
  Agendada: "bg-secondary text-secondary-foreground",
  Pendiente: "bg-amber-100 text-amber-800",
  Completada: "bg-emerald-100 text-emerald-800",
  Cancelada: "bg-red-100 text-red-700",
}

export function AppointmentTable() {
  const [isModalOpen, setIsModalOpen]         = useState(false)
  const [selectedAppointment, setSelected]    = useState<Appointment | null>(null)
  const [page, setPage]                       = useState(1)

  const { data, isLoading } = useAppointments(page, PAGE_SIZE, "")
  const { data: states }    = useAppointmentStates()
  const changeState = useChangeAppointmentState()

  const appointments = data?.items ?? []
  const total        = data?.total ?? 0
  const totalPages   = Math.ceil(total / PAGE_SIZE)
  const appointmentStates = states ?? []

  if (isLoading) {
    return (
      <TableSkeleton
        title="Lista de Citas"
        columns={5}
        columnWidths={["w-24", "w-32", "w-28", "w-20", "w-40"]}
      />
    )
  }

  const handleEdit = (appointment: Appointment) => {
    setSelected(appointment)
    setIsModalOpen(true)
  }

  const handleNew = () => {
    setSelected(null)
    setIsModalOpen(true)
  }

  const handleStateChange = (appointment: Appointment, newStateName: string) => {
    const newState = appointmentStates.find((s) => s.name === newStateName)
    if (!newState) return
    changeState.mutate({ id: appointment.id, stateId: newState.id })
  }

  return (
    <>
      <div className="rounded-lg border border-border bg-card p-6 shadow-md">

        {/* Header */}
        <div className="flex items-center justify-between mb-4">
          <div className="flex items-center gap-2">
            <CalendarDays className="h-5 w-5 text-primary" />
            <h2 className="text-xl font-bold text-foreground">Lista de Citas</h2>
          </div>
          <button
            onClick={handleNew}
            className="flex items-center gap-2 bg-primary text-primary-foreground px-4 py-2 rounded-lg text-sm hover:opacity-90 transition"
          >
            <Plus className="h-4 w-4" />
            Nueva cita
          </button>
        </div>

        {/* Tabla */}
        <table className="w-full text-left">
          <thead>
            <tr className="bg-muted">
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Fecha</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Dueño</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Mascota</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Estado</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Observación</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {appointments.map((appointment) => (
              <tr
                key={appointment.id}
                className="border-b border-border hover:bg-accent transition-colors"
              >
                <td className="py-4 px-4 text-foreground font-medium">
                  {new Date(appointment.date).toLocaleDateString("es-CO")}
                </td>
                <td className="py-4 px-4 text-muted-foreground">{appointment.ownerName}</td>
                <td className="py-4 px-4 text-muted-foreground">{appointment.petName}</td>
                <td className="py-4 px-4">
                  <Select
                    value={appointment.state}
                    onValueChange={(value) => handleStateChange(appointment, value)}
                  >
                    <SelectTrigger
                      className={`w-[130px] h-7 text-xs font-semibold rounded-full border-none focus:ring-0 ${
                        STATE_COLORS[appointment.state] ?? "bg-muted text-muted-foreground"
                      }`}
                    >
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      {appointmentStates.map((s) => (
                        <SelectItem key={s.id} value={s.name}>
                          {s.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </td>
                <td className="py-4 px-4 text-muted-foreground">{appointment.observation}</td>
                <td className="py-4 px-4">
                  <button
                    onClick={() => handleEdit(appointment)}
                    className="flex items-center gap-1 bg-secondary text-secondary-foreground text-xs font-semibold px-3 py-1 rounded-full hover:opacity-90 transition"
                  >
                    <Pencil className="h-3 w-3" />
                    Editar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {/* Paginación */}
        {totalPages > 1 && (
          <div className="flex items-center justify-between border-t border-border px-2 py-3 mt-2 text-sm text-muted-foreground">
            <span>
              Mostrando {(page - 1) * PAGE_SIZE + 1}–{Math.min(page * PAGE_SIZE, total)} de {total} citas
            </span>
            <div className="flex gap-1">
              <button
                disabled={page === 1}
                onClick={() => setPage((p) => p - 1)}
                className="px-3 py-1 rounded border border-border hover:bg-muted disabled:opacity-40 transition"
              >
                ←
              </button>
              {Array.from({ length: totalPages }, (_, i) => i + 1).map((p) => (
                <button
                  key={p}
                  onClick={() => setPage(p)}
                  className={`px-3 py-1 rounded border transition ${
                    p === page
                      ? "bg-primary text-primary-foreground border-primary"
                      : "border-border hover:bg-muted"
                  }`}
                >
                  {p}
                </button>
              ))}
              <button
                disabled={page === totalPages}
                onClick={() => setPage((p) => p + 1)}
                className="px-3 py-1 rounded border border-border hover:bg-muted disabled:opacity-40 transition"
              >
                →
              </button>
            </div>
          </div>
        )}
      </div>

      <AppointmentFormModal
        open={isModalOpen}
        onOpenChange={(val) => {
          setIsModalOpen(val)
          if (!val) setSelected(null)
        }}
        selected={selectedAppointment}
      />
    </>
  )
}