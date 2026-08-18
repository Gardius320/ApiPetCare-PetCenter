"use client"

import Link from "next/link"
import { AlertTriangle } from "lucide-react"
import { useSupplyStats } from "@/features/supply/hook/use-supplies"

export function LowStockCard() {
  const { data: stats, isLoading } = useSupplyStats()

  return (
    <Link
      href="/Supply"
      className="rounded-lg border border-border bg-card p-5 flex flex-col gap-3 hover:border-primary transition-colors"
    >
      <div className="flex items-start justify-between">
        <p className="text-xs text-muted-foreground">Insumos con stock bajo</p>
        <AlertTriangle className="size-5 shrink-0 text-[#E0A458]" />
      </div>
      <p className="font-mono font-medium text-2xl leading-none text-foreground">
        {isLoading ? "…" : stats?.lowStock ?? 0}
      </p>
    </Link>
  )
}
