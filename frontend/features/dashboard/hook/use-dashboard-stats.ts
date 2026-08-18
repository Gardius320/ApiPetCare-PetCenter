"use client"

import { useMemo } from "react"
import { useAllOwners } from "@/features/owners/hooks/use-owners"
import { usePets } from "@/features/pets/hooks/use-pets"
import { useAppointments } from "@/features/appointments/hooks/use-appointments"
import { useInvoices } from "@/features/invoices/hooks/use-invoices"
import type { Pet } from "@/features/pets/types/pet.types"
import type { Appointment } from "@/features/appointments/types/appointment.types"
import type { Invoice } from "@/features/invoices/types/invoice.types"

export function useDashboardStats() {
  const { data: ownersData, isLoading: loadingOwners } = useAllOwners()
  const { data: petsData, isLoading: loadingPets } = usePets(1, 100, "")
  const { data: appointmentsData, isLoading: loadingAppointments } = useAppointments(1, 100, "")
  const { data: invoicesData, isLoading: loadingInvoices } = useInvoices()

  const isLoading = loadingOwners || loadingPets || loadingAppointments || loadingInvoices

  const stats = useMemo(() => {
    const pets = petsData?.items ?? []
    const owners = ownersData?.items ?? []
    const appointments = appointmentsData?.items ?? []
    const invoices = invoicesData ?? []

    const activePets = pets.filter((p: Pet) => p.isActive).length
    const inactivePets = pets.filter((p: Pet) => !p.isActive).length

    const speciesCount = pets.reduce((acumulador, pet: Pet) => {
      acumulador[pet.species] = (acumulador[pet.species] || 0) + 1
      return acumulador
    }, {} as Record<string, number>)

    const speciesChartData = Object.entries(speciesCount).map(([species, count]) => ({
      status: species,
      count,
    }))

    const revenueByMonth = invoices.reduce((acumulador, invoice: Invoice) => {
      const fecha = new Date(invoice.issueDate)
      const clave = `${fecha.getFullYear()}-${String(fecha.getMonth() + 1).padStart(2, "0")}`
      const etiqueta = fecha.toLocaleDateString("es-CO", { month: "short", year: "2-digit" })

      if (!acumulador[clave]) {
        acumulador[clave] = { label: etiqueta, total: 0 }
      }
      acumulador[clave].total += invoice.total

      return acumulador
    }, {} as Record<string, { label: string; total: number }>)

    const invoicesChartData = Object.entries(revenueByMonth)
      .sort(([claveA], [claveB]) => claveA.localeCompare(claveB))
      .map(([, { label, total }]) => ({
        status: label,
        count: total,
      }))

    const recentAppointments = [...appointments]
      .sort((a: Appointment, b: Appointment) => new Date(b.date).getTime() - new Date(a.date).getTime())
      .slice(0, 5)

    return {
      totalPets: pets.length,
      totalOwners: owners.length,
      totalAppointments: appointments.length,
      activePets,
      inactivePets,
      recentAppointments,
      chartData: [
        { status: "Activos", count: activePets },
        { status: "Inactivos", count: inactivePets },
      ],
      speciesChartData,
      invoicesChartData,
    }
  }, [petsData, ownersData, appointmentsData, invoicesData])

  return { ...stats, isLoading }
}