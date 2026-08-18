import { Package, PackageX, AlertTriangle, Layers } from 'lucide-react'
import type { SupplyStats } from '../types/supply.types'

interface SuppliesStatsProps {
  stats: SupplyStats
}

export function SuppliesStats({ stats }: SuppliesStatsProps) {
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
      <div className="rounded-lg border border-border bg-card p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-muted-foreground">Total de insumos</p>
            <p className="text-2xl font-bold font-mono text-foreground">{stats.total}</p>
          </div>
          <Package className="h-8 w-8 text-primary" />
        </div>
      </div>

      <div className="rounded-lg border border-border bg-card p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-muted-foreground">Bajo stock</p>
            <p className="text-2xl font-bold font-mono text-foreground">{stats.lowStock}</p>
          </div>
          <AlertTriangle className="h-8 w-8 text-[#E0A458]" />
        </div>
      </div>

      <div className="rounded-lg border border-border bg-card p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-muted-foreground">Agotados</p>
            <p className="text-2xl font-bold font-mono text-foreground">{stats.outOfStock}</p>
          </div>
          <PackageX className="h-8 w-8 text-destructive" />
        </div>
      </div>

      <div className="rounded-lg border border-border bg-card p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-muted-foreground">Categorías en uso</p>
            <p className="text-2xl font-bold font-mono text-foreground">{stats.categoriesInUse}</p>
          </div>
          <Layers className="h-8 w-8 text-[#FF8C6B]" />
        </div>
      </div>
    </div>
  )
}