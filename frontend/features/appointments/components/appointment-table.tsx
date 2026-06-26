"use client"

import { useState } from "react"
import { CalendarDays, Plus, Pencil } from "lucide-react"
import { AppointmentFormModal } from "./appointment-form-modal"
import { useAppointments } from "../hooks/use-appointments"
import type { Appointment } from "../types/appointment.types"

const PAGE_SIZE = 10

export function AppointmentTable() {
  const [isModalOpen, setIsModalOpen]         = useState(false)
  const [selectedAppointment, setSelected]    = useState<Appointment | null>(null)
  const [page, setPage]                       = useState(1)

  const { data, isLoading } = useAppointments(page, PAGE_SIZE, "")
  const appointments = data?.items ?? []
  const total        = data?.total ?? 0
  const totalPages   = Math.ceil(total / PAGE_SIZE)

  if (isLoading) return <p className="p-6 text-gray-500">Cargando citas...</p>

  const handleEdit = (appointment: Appointment) => {
    setSelected(appointment)
    setIsModalOpen(true)
  }

  const handleNew = () => {
    setSelected(null)
    setIsModalOpen(true)
  }

  return (
    <>
      <div className="rounded-2xl border border-gray-200 bg-white p-6 shadow-md">

        {/* Header */}
        <div className="flex items-center justify-between mb-4">
          <div className="flex items-center gap-2">
            <CalendarDays className="h-5 w-5 text-primary" />
            <h2 className="text-xl font-bold text-gray-800">Lista de Citas</h2>
          </div>
          <button
            onClick={handleNew}
            className="flex items-center gap-2 bg-blue-400 text-white px-4 py-2 rounded-lg text-sm hover:opacity-90 transition"
          >
            <Plus className="h-4 w-4" />
            Nueva cita
          </button>
        </div>

        {/* Tabla */}
        <table className="w-full text-left">
          <thead>
            <tr className="bg-gray-50">
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Fecha</th>
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Dueño</th>
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Mascota</th>
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Estado</th>
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Observación</th>
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {appointments.map((appointment) => (
              <tr
                key={appointment.id}
                className="border-b border-gray-100 hover:bg-blue-50 transition-colors"
              >
                <td className="py-4 px-4 text-gray-700 font-medium">
                  {new Date(appointment.fecha).toLocaleDateString("es-CO")}
                </td>
                <td className="py-4 px-4 text-gray-600">{appointment.nombreDueno}</td>
                <td className="py-4 px-4 text-gray-600">{appointment.nombreMascota}</td>
                <td className="py-4 px-4">
                  <span className="bg-blue-100 text-blue-700 text-xs font-semibold px-3 py-1 rounded-full">
                    {appointment.estado}
                  </span>
                </td>
                <td className="py-4 px-4 text-gray-600">{appointment.observacion}</td>
                <td className="py-4 px-4">
                  <button
                    onClick={() => handleEdit(appointment)}
                    className="flex items-center gap-1 bg-yellow-100 text-yellow-700 text-xs font-semibold px-3 py-1 rounded-full hover:bg-yellow-200 transition"
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
          <div className="flex items-center justify-between border-t border-gray-200 px-2 py-3 mt-2 text-sm text-gray-500">
            <span>
              Mostrando {(page - 1) * PAGE_SIZE + 1}–{Math.min(page * PAGE_SIZE, total)} de {total} citas
            </span>
            <div className="flex gap-1">
              <button
                disabled={page === 1}
                onClick={() => setPage((p) => p - 1)}
                className="px-3 py-1 rounded border border-gray-200 hover:bg-gray-50 disabled:opacity-40 transition"
              >
                ←
              </button>
              {Array.from({ length: totalPages }, (_, i) => i + 1).map((p) => (
                <button
                  key={p}
                  onClick={() => setPage(p)}
                  className={`px-3 py-1 rounded border transition ${
                    p === page
                      ? "bg-blue-400 text-white border-blue-400"
                      : "border-gray-200 hover:bg-gray-50"
                  }`}
                >
                  {p}
                </button>
              ))}
              <button
                disabled={page === totalPages}
                onClick={() => setPage((p) => p + 1)}
                className="px-3 py-1 rounded border border-gray-200 hover:bg-gray-50 disabled:opacity-40 transition"
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