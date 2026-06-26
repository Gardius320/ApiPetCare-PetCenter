"use client"

import { PetsStats } from "@/features/pets/components/pets-stats"
import { PetsTable } from "@/features/pets/components/pets-table"
import { usePets } from "@/features/pets/hooks/use-pets"

export default function PetsPage() {
  const { data, isLoading } = usePets(1, 100, "")
  const pets = data?.items ?? []

  return (
    <div className="space-y-6">
      {/* Encabezado */}
      <div>
        <h1 className="text-2xl font-bold text-foreground">Mascotas</h1>
        <p className="text-sm text-muted-foreground mt-0.5">
          Gestiona las mascotas registradas en el sistema.
        </p>
      </div>

      {/* Estadísticas */}
      <PetsStats pets={pets} isLoading={isLoading} />

      {/* Tabla */}
      <PetsTable />
    </div>
  )
}