"use client"

import { useState } from "react"
import { ServiceTable } from "@/features/services/components/service-table"
import { useServices, useDeleteService } from "@/features/services/hooks/use-service"

export default function ServicesPage() {
  const [search, setSearch] = useState("")
  const { data, isLoading } = useServices(1, 100, search)
  const services = data ?? []
  const deleteService = useDeleteService()

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-foreground">Servicios</h1>
        <p className="text-sm text-muted-foreground mt-0.5">
          Gestiona los servicios ofrecidos en el sistema.
        </p>
      </div>

      <ServiceTable
        services={services}
        isLoading={isLoading}
        search={search}
        onSearchChange={setSearch}
        onDelete={(id) => deleteService.mutate(id)}
      />
    </div>
  )
}
