"use client"

import { OwnersTable } from "@/features/owners/components/owners-table"
import { useOwners, useDeleteOwner } from "@/features/owners/hooks/use-owners"

export default function OwnersPage() {
  const { data, isLoading } = useOwners()
const owners = data?.items ?? []
  const deleteOwner = useDeleteOwner()

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-foreground">Propietarios</h1>
        <p className="text-sm text-muted-foreground mt-0.5">
          Gestiona los propietarios registrados en el sistema.
        </p>
      </div>

      <OwnersTable
        owners={owners}
        isLoading={isLoading}
        onDelete={(id) => deleteOwner.mutate(id)}
      />
    </div>
  )
}
