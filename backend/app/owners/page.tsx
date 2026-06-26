'use client'
import OwnersTable from '@/features/owners/components/owners-table'

export default function OwnersPage() {
  return (
    <main className="flex-1 overflow-auto p-4 md:p-6">
      <div className="mx-auto max-w-7xl space-y-6">
        <div>
          <h1 className="text-2xl font-bold text-foreground">Propietarios</h1>
          <p className="text-sm text-muted-foreground">Gestiona los propietarios</p>
        </div>
        <OwnersTable />
      </div>
    </main>
  )
}