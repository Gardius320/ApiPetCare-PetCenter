"use client"

import { useDashboardStats } from "@/features/dashboard/hook/use-dashboard-stats"
import { DashboardStats } from "@/features/dashboard/components/dashboard-stats"
import { DashboardChart } from "@/features/dashboard/components/dashboard-chart"
import { TableSkeleton } from "@/components/shared/table-skeleton"

export default function DashboardPage() {
  const { isLoading, chartData, ...stats } = useDashboardStats()

  if (isLoading) {
    return <TableSkeleton title="Cargando Dashboard..." columns={3} showActions={false} />
  }

  return (
    <div className="space-y-6 p-6">
      <h1 className="text-2xl font-bold text-gray-800">Dashboard</h1>
      <DashboardStats
        totalPets={stats.totalPets}
        totalOwners={stats.totalOwners}
        totalAppointments={stats.totalAppointments}
      />
      <DashboardChart data={chartData} />
    </div>
  )
}