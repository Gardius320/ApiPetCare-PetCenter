"use client"

import { useMemo } from "react"
import { useAllOwners } from "@/features/owners/hooks/use-owners"
import { usePets } from "@/features/pets/hooks/use-pets"
import { useAppointments } from "@/features/appointments/hooks/use-appointments"
import type { Pet } from "@/features/pets/types/pet.types"
import type { Appointment } from "@/features/appointments/types/appointment.types"

export function useDashboardStats() {
  const { data: ownersData, isLoading: loadingOwners } = useAllOwners()
  const { data: petsData, isLoading: loadingPets } = usePets(1, 100, "")
  const { data: appointmentsData, isLoading: loadingAppointments } = useAppointments(1, 100, "")

  const isLoading = loadingOwners || loadingPets || loadingAppointments

  const stats = useMemo(() => {
    const pets = petsData?.items ?? []
    const owners = ownersData?.items ?? []
    const appointments = appointmentsData?.items ?? []

    const activePets = pets.filter((p: Pet) => p.isActive).length
    const inactivePets = pets.filter((p: Pet) => !p.isActive).length

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
    }
  }, [petsData, ownersData, appointmentsData])

  return { ...stats, isLoading }
}