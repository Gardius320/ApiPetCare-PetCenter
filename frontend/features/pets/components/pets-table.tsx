"use client"

import { useState } from "react"
import { PawPrint, Plus, Dog, Cat, Bird, Rabbit, Fish } from "lucide-react"
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
      <div className="rounded-2xl border border-gray-200 bg-white p-6 shadow-md">

        {/* Header */}
        <div className="flex items-center justify-between mb-4">
          <div className="flex items-center gap-2">
            <PawPrint className="h-5 w-5 text-primary" />
            <h2 className="text-xl font-bold text-gray-800">Lista de Mascotas</h2>
          </div>
          <button
            onClick={() => setIsModalOpen(true)}
            className="flex items-center gap-2 bg-blue-400 text-white px-4 py-2 rounded-lg text-sm hover:opacity-90 transition"
          >
            <Plus className="h-4 w-4" />
            Nueva mascota
          </button>
        </div>

        {/* Tabla */}
        <table className="w-full text-left">
          <thead>
            <tr className="bg-gray-50">
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Nombre</th>
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Especie</th>
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Propietario</th>
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Email</th>
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Estado</th>
              <th className="border-b border-gray-200 py-3 px-4 text-sm text-gray-600">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {pets.map((pet) => {
              return (
                <tr
                  key={pet.id}
                  className="border-b border-gray-100 hover:bg-blue-50 transition-colors"
                >
                  <td className="py-4 px-4 text-gray-700 font-medium">{pet.name}</td>

                  <td className="py-4 px-4 text-gray-600">
                    <span className="flex items-center gap-1.5">
                      <SpeciesIcon species={pet.species} />
                      {pet.species}
                    </span>
                  </td>

                  <td className="py-4 px-4 text-gray-600">{pet.ownerName}</td>
                  <td className="py-4 px-4 text-gray-600">{pet.emailOwner}</td>

                  {/* Badge Estado */}
                  <td className="py-4 px-4">
                    {pet.isActive ? (
                      <span className="bg-green-100 text-green-700 text-xs font-semibold px-3 py-1 rounded-full">
                        Activo
                      </span>
                    ) : (
                      <span className="bg-red-100 text-red-700 text-xs font-semibold px-3 py-1 rounded-full">
                        Inactivo
                      </span>
                    )}
                  </td>

                  {/* Botón Acción */}
                  <td className="py-4 px-4">
                    <button
                      onClick={() => changePetState.mutate({ id: pet.id, isActive: !pet.isActive })}
                      disabled={changePetState.isPending}
                      className={pet.isActive
                        ? "bg-red-100 text-red-700 text-xs font-semibold px-3 py-1 rounded-full hover:bg-red-200 transition"
                        : "bg-green-100 text-green-700 text-xs font-semibold px-3 py-1 rounded-full hover:bg-green-200 transition"
                      }
                    >
                      {pet.isActive ? "Inactivar" : "Activar"}
                    </button>
                  </td>
                </tr>
              )
            })}
          </tbody>
        </table>

        {/* Paginación */}
        {totalPages > 1 && (
          <div className="flex items-center justify-between border-t border-gray-200 px-2 py-3 mt-2 text-sm text-gray-500">
            <span>
              Mostrando {(page - 1) * PAGE_SIZE + 1}–{Math.min(page * PAGE_SIZE, total)} de {total} mascotas
            </span>
            <div className="flex gap-1">
              <button
                disabled={page === 1}
                onClick={() => setPage((p) => p - 1)}
                className="px-3 py-1 rounded border border-gray-200 hover:bg-gray-50 disabled:opacity-40 transition"
              >
                ←
              </button>

              {Array.from({ length: totalPages }, (_, i) => i + 1).map((p) => (
                <button
                  key={p}
                  onClick={() => setPage(p)}
                  className={`px-3 py-1 rounded border transition ${
                    p === page
                      ? "bg-blue-400 text-white border-blue-400"
                      : "border-gray-200 hover:bg-gray-50"
                  }`}
                >
                  {p}
                </button>
              ))}

              <button
                disabled={page === totalPages}
                onClick={() => setPage((p) => p + 1)}
                className="px-3 py-1 rounded border border-gray-200 hover:bg-gray-50 disabled:opacity-40 transition"
              >
                →
              </button>
            </div>
          </div>
        )}
      </div>

      {/* Modal */}
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