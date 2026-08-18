"use client"

import { useState } from "react"
import { Users, Plus, Pencil } from "lucide-react"
import { OwnerFormModal } from "./owner-form-modal"
import { useCreateOwner, useUpdateOwner } from "../hooks/use-owners"
import type { Owner, CreateOwnerDto, UpdateOwnerDto } from "../types/owner.types"
import { TableSkeleton } from "@/components/shared/table-skeleton"

const PAGE_SIZE = 10

interface OwnersTableProps {
  owners: Owner[]
  isLoading: boolean
  onDelete: (id: number) => void
}

export function OwnersTable({ owners, isLoading, onDelete }: OwnersTableProps) {
  const [page, setPage]                   = useState(1)
  const [isModalOpen, setIsModalOpen]     = useState(false)
  const [editingOwner, setEditingOwner]   = useState<Owner | undefined>(undefined)

  const createOwner = useCreateOwner()
  const updateOwner = useUpdateOwner()
  const isSaving    = createOwner.isPending || updateOwner.isPending

  const totalPages = Math.max(1, Math.ceil(owners.length / PAGE_SIZE))
  const paginated  = owners.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE)

  function handleEdit(owner: Owner) {
    setEditingOwner(owner)
    setIsModalOpen(true)
  }

  function handleOpenChange(open: boolean) {
    setIsModalOpen(open)
    if (!open) setEditingOwner(undefined)
  }

  function handleSave(dto: CreateOwnerDto | UpdateOwnerDto, id?: number) {
    if (id !== undefined) {
      updateOwner.mutate(
        { id, dto: dto as UpdateOwnerDto },
        { onSuccess: () => handleOpenChange(false) }
      )
    } else {
      createOwner.mutate(dto as CreateOwnerDto, {
        onSuccess: () => handleOpenChange(false),
      })
    }
  }

  if (isLoading) {
  return (
    <TableSkeleton
      title="Lista de Propietarios"
      columns={4}
      columnWidths={["w-32", "w-40", "w-24", "w-16"]}
    />
  )
}

  return (
    <>
      <div className="rounded-lg border border-border bg-card p-6 shadow-md">

        {/* Header */}
        <div className="flex items-center justify-between mb-4">
          <div className="flex items-center gap-2">
            <Users className="h-5 w-5 text-primary" />
            <h2 className="text-xl font-bold text-foreground">Lista de Propietarios</h2>
          </div>
          <button
            onClick={() => { setEditingOwner(undefined); setIsModalOpen(true) }}
            className="flex items-center gap-2 bg-primary text-primary-foreground px-4 py-2 rounded-lg text-sm hover:opacity-90 transition"
          >
            <Plus className="h-4 w-4" />
            Nuevo propietario
          </button>
        </div>

        {/* Tabla */}
        <table className="w-full text-left">
          <thead>
            <tr className="bg-muted">
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Nombre</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Email</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Teléfono</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Género</th>
              <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Acciones</th>
            </tr>
          </thead>
          <tbody>

            {/* Empty */}
            {paginated.length === 0 && (
              <tr>
                <td colSpan={5} className="py-16 text-center text-sm text-muted-foreground">
                  <Users className="mx-auto mb-2 size-8 opacity-25" />
                  No se encontraron propietarios
                </td>
              </tr>
            )}

            {/* Filas */}
            {paginated.map((owner) => (
              <tr
                key={owner.id}
                className="border-b border-border hover:bg-accent transition-colors"
              >
                <td className="py-4 px-4 text-foreground font-medium">{owner.ownerName}</td>
                <td className="py-4 px-4 text-muted-foreground">{owner.email}</td>
                <td className="py-4 px-4 text-muted-foreground">{owner.phoneNumber}</td>
                <td className="py-4 px-4 text-muted-foreground">{owner.gender}</td>

                {/* Acciones */}
                <td className="py-4 px-4">
                  <div className="flex items-center gap-2">
                    <button
                      onClick={() => handleEdit(owner)}
                      className="bg-secondary text-secondary-foreground text-xs font-semibold px-3 py-1 rounded-full hover:opacity-90 transition"
                    >
                      <Pencil className="h-3 w-3 inline mr-1" />
                      Editar
                    </button>
                    <button
                      onClick={() => {
                        if (window.confirm(`¿Eliminar a ${owner.ownerName}?`)) {
                          onDelete(owner.id)
                        }
                      }}
                      className="bg-destructive/10 text-destructive text-xs font-semibold px-3 py-1 rounded-full hover:bg-destructive/20 transition"                   >
                      Eliminar
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {/* Paginación */}
        {totalPages > 1 && (
          <div className="flex items-center justify-between border-t border-border px-2 py-3 mt-2 text-sm text-muted-foreground">
            <span>
              Mostrando {(page - 1) * PAGE_SIZE + 1}–{Math.min(page * PAGE_SIZE, owners.length)} de {owners.length} propietarios
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

      {/* Modal */}
      <OwnerFormModal
        open={isModalOpen}
        onOpenChange={handleOpenChange}
        isSaving={isSaving}
        onSave={handleSave}
        owner={editingOwner}
      />
    </>
  )
}