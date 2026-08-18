"use client"

import { useState } from "react"
import { useRouter } from "next/navigation"
import { PawPrint, Plus, Dog, Cat, Bird, Rabbit, Fish, Stethoscope } from "lucide-react"
import { PetFormModal } from "./pet-form-modal"
import { useCreatePet, useChangePetState, usePets } from "../hooks/use-pets"
import { TableSkeleton } from "@/components/shared/table-skeleton"

function SpeciesIcon({ species }: { species: string }) {
  const props = { className: "w-4 h-4", strokeWidth: 1.5 }
  if (species === "Perro")  return <Dog    {...props} />
  if (species === "Gato")   return <Cat    {...props} />
  if (species === "Ave")    return <Bird   {...props} />
  if (species === "Conejo") return <Rabbit {...props} />
  if (species === "Pez")    return <Fish   {...props} />
  return <PawPrint {...props} />
}

const PAGE_SIZE = 10

export function PetsTable() {
  const router = useRouter()
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [page, setPage]               = useState(1)

  const { data, isLoading } = usePets(page, PAGE_SIZE, "")
  const pets       = data?.items ?? []
  const total      = data?.total ?? 0
  const totalPages = Math.ceil(total / PAGE_SIZE)

  const createPet      = useCreatePet()
  const changePetState = useChangePetState()

  if (isLoading) {
  return (
    <TableSkeleton
      title="Lista de Mascotas"
      columns={5}
      columnWidths={["w-28", "w-20", "w-32", "w-40", "w-20"]}
    />
  )
}

  return (
    <>
      <div className="rounded-lg border border-border bg-card p-6 shadow-md">

        {/* Header */}
        <div className="flex items-center justify-between mb-4">
          <div className="flex items-center gap-2">
            <PawPrint className="h-5 w-5 text-primary" />
            <h2 className="text-xl font-bold text-foreground">Lista de Mascotas</h2>
          </div>
          <button
            onClick={() => setIsModalOpen(true)}
            className="flex items-center gap-2 bg-primary text-primary-foreground px-4 py-2 rounded-lg text-sm hover:opacity-90 transition"
          >
            <Plus className="h-4 w-4" />
            Nueva mascota
          </button>
        </div>

        {/* Tabla */}
        <table className="w-full text-left">
          <thead>
            <tr className="bg-muted">
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Nombre</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Especie</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Propietario</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Email</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Estado</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {pets.map((pet) => {
              return (
                <tr
                  key={pet.id}
                  className="border-b border-border hover:bg-accent transition-colors"
                >
                  <td className="py-4 px-4 text-foreground font-medium">{pet.name}</td>

                  <td className="py-4 px-4 text-muted-foreground">
                    <span className="flex items-center gap-1.5">
                      <SpeciesIcon species={pet.species} />
                      {pet.species}
                    </span>
                  </td>

                  <td className="py-4 px-4 text-muted-foreground">{pet.ownerName}</td>
                  <td className="py-4 px-4 text-muted-foreground">{pet.emailOwner}</td>

                  {/* Badge Estado */}
                  <td className="py-4 px-4">
                    {pet.isActive ? (
                      <span className="bg-emerald-100 text-emerald-800 text-xs font-semibold px-3 py-1 rounded-full">
                        Activo
                      </span>
                    ) : (
                      <span className="bg-red-100 text-red-700 text-xs font-semibold px-3 py-1 rounded-full">
                        Inactivo
                      </span>
                    )}
                  </td>

                  {/* Botones Acción */}
                  <td className="py-4 px-4">
                    <div className="flex items-center gap-2">
                      <button
                        onClick={() => router.push(`/medical-records?petId=${pet.id}`)}
                        title="Ver historial clínico"
                        className="flex items-center gap-1 bg-secondary text-secondary-foreground text-xs font-semibold px-3 py-1 rounded-full hover:opacity-90 transition"
                      >
                        <Stethoscope className="h-3.5 w-3.5" />
                        Historial
                      </button>

                      <button
                        onClick={() => changePetState.mutate({ id: pet.id, isActive: !pet.isActive })}
                        disabled={changePetState.isPending}
                        className={pet.isActive
                          ? "bg-destructive/10 text-destructive text-xs font-semibold px-3 py-1 rounded-full hover:bg-destructive/20 transition"
                          : "bg-emerald-100 text-emerald-800 text-xs font-semibold px-3 py-1 rounded-full hover:bg-emerald-200 transition"
                        }
                      >
                        {pet.isActive ? "Inactivar" : "Activar"}
                      </button>
                    </div>
                  </td>
                </tr>
              )
            })}
          </tbody>
        </table>

        {/* Paginación */}
        {totalPages > 1 && (
          <div className="flex items-center justify-between border-t border-border px-2 py-3 mt-2 text-sm text-muted-foreground">
            <span>
              Mostrando {(page - 1) * PAGE_SIZE + 1}–{Math.min(page * PAGE_SIZE, total)} de {total} mascotas
            </span>
            <div className="flex gap-1">
              <button
                disabled={page === 1}
                onClick={() => setPage((p) => p - 1)}
                className="px-3 py-1 rounded border border-border hover:bg-muted disabled:opacity-40 transition"
              >
                ←
              </button>

              {Array.from({ length: totalPages }, (_, i) => i + 1).map((p) => (
                <button
                  key={p}
                  onClick={() => setPage(p)}
                  className={`px-3 py-1 rounded border transition ${
                    p === page
                      ? "bg-primary text-primary-foreground border-primary"
                      : "border-border hover:bg-muted"
                  }`}
                >
                  {p}
                </button>
              ))}

              <button
                disabled={page === totalPages}
                onClick={() => setPage((p) => p + 1)}
                className="px-3 py-1 rounded border border-border hover:bg-muted disabled:opacity-40 transition"
              >
                →
              </button>
            </div>
          </div>
        )}
      </div>

      {/* Modal crear mascota */}
      <PetFormModal
        open={isModalOpen}
        onOpenChange={setIsModalOpen}
        isSaving={createPet.isPending}
        onSave={(dto) => {
          createPet.mutate(dto, { onSuccess: () => setIsModalOpen(false) })
        }}
      />
    </>
  )
}