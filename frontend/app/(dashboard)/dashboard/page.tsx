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
    <div className="-m-6 min-h-full space-y-6 bg-[#F5F7F4] p-6 font-[family-name:var(--font-inter)]">
      <div className="relative overflow-hidden">
        {/* eslint-disable-next-line @next/next/no-img-element */}
        <img
          src="/decorations/blob-cloud.svg"
          alt=""
          aria-hidden="true"
          className="pointer-events-none select-none absolute -top-8 -right-8 h-40 w-40 opacity-40"
        />
        <div className="relative flex items-center gap-1.5">
          <span className="h-[6px] w-[6px] rounded-full bg-[#1F6F5C]" />
          <span className="text-[11px] font-medium uppercase tracking-wide text-[#1F6F5C]">
            Gestión veterinaria
          </span>
        </div>
        <h1 className="relative mt-1 font-[family-name:var(--font-space-grotesk)] font-medium text-2xl text-[#16302B]">
          Dashboard
        </h1>
      </div>
      <DashboardStats
        totalPets={stats.totalPets}
        totalOwners={stats.totalOwners}
        totalAppointments={stats.totalAppointments}
       
      />
      <DashboardChart data={chartData} />
    </div>
  )
}