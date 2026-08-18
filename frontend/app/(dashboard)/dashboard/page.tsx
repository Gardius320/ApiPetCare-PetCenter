"use client"

import { useDashboardStats } from "@/features/dashboard/hook/use-dashboard-stats"
import { DashboardStats } from "@/features/dashboard/components/dashboard-stats"
import { DashboardChart } from "@/features/dashboard/components/dashboard-chart"
import { LowStockCard } from "@/features/dashboard/components/low-stock-card"
import { TableSkeleton } from "@/components/shared/table-skeleton"
import Link from "next/link"

export default function DashboardPage() {
  const { isLoading, chartData, speciesChartData,invoicesChartData, ...stats } = useDashboardStats()

  if (isLoading) {
    return <TableSkeleton title="Cargando Dashboard..." columns={3} showActions={false} />
  }

  return (
    <div className="-m-6 min-h-full space-y-6 bg-muted p-6">
      <div className="relative overflow-hidden">
        {/* eslint-disable-next-line @next/next/no-img-element */}
        <img
          src="/decorations/blob-cloud.svg"
          alt=""
          aria-hidden="true"
          className="pointer-events-none select-none absolute -top-8 -right-8 h-40 w-40 opacity-40"
        />
        <div className="relative flex items-center gap-1.5">
          <span className="h-[6px] w-[6px] rounded-full bg-primary" />
          <span className="text-[11px] font-medium uppercase tracking-wide text-primary">
            Gestión veterinaria
          </span>
        </div>
        <h1 className="relative mt-1 font-heading font-medium text-2xl text-foreground">
          Dashboard
        </h1>
      </div>
      <DashboardStats
        totalPets={stats.totalPets}
        totalOwners={stats.totalOwners}
        totalAppointments={stats.totalAppointments}
      />
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
        <LowStockCard />
      </div>
      <DashboardChart title="Mascotas por estado" data={chartData} />
      <DashboardChart title="Mascotas por especie" data={speciesChartData} />
      <DashboardChart title="Facturación por mes" data={invoicesChartData} />
    </div>
  )
}