'use client'

import { useState } from 'react'
import { useSupplies, useSupplyStats } from '@/features/supply/hook/use-supplies'
import { SuppliesStats } from '@/features/supply/components/supplies-stats'
import { SuppliesTable } from '@/features/supply/components/supplies-table'

const PAGE_SIZE = 10

export default function SupplyPage() {
  const [page, setPage] = useState(1)

  const { data: stats, isLoading: loadingStats } = useSupplyStats()
  const { data: suppliesData, isLoading: loadingSupplies } = useSupplies(page, PAGE_SIZE)

  return (
    <div className="space-y-6 p-6">
      <h1 className="text-2xl font-bold text-foreground">Insumos</h1>

      {loadingStats && <p className="text-sm text-muted-foreground">Cargando estadísticas...</p>}
      {stats && <SuppliesStats stats={stats} />}

      <SuppliesTable
        supplies={suppliesData?.items ?? []}
        totalRecords={suppliesData?.totalRecords ?? 0}
        page={page}
        pageSize={PAGE_SIZE}
        isLoading={loadingSupplies}
        onPageChange={setPage}
      />
    </div>
  )
}