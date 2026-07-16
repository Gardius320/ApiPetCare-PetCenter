import { Package, PackageX, AlertTriangle, Layers } from 'lucide-react'
import type { SupplyStats } from '../types/supply.types'

interface SuppliesStatsProps {
  stats: SupplyStats
}

export function SuppliesStats({ stats }: SuppliesStatsProps) {
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
      <div className="rounded-lg border bg-white p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-500">Total de insumos</p>
            <p className="text-2xl font-bold">{stats.total}</p>
          </div>
          <Package className="h-8 w-8 text-blue-500" />
        </div>
      </div>

      <div className="rounded-lg border bg-white p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-500">Bajo stock</p>
            <p className="text-2xl font-bold">{stats.bajoStock}</p>
          </div>
          <AlertTriangle className="h-8 w-8 text-orange-500" />
        </div>
      </div>

      <div className="rounded-lg border bg-white p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-500">Agotados</p>
            <p className="text-2xl font-bold">{stats.agotados}</p>
          </div>
          <PackageX className="h-8 w-8 text-red-500" />
        </div>
      </div>

      <div className="rounded-lg border bg-white p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-500">Categorías en uso</p>
            <p className="text-2xl font-bold">{stats.categoriasEnUso}</p>
          </div>
          <Layers className="h-8 w-8 text-purple-500" />
        </div>
      </div>
    </div>
  )
}