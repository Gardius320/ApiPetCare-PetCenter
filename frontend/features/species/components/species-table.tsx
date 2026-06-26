"use client"

import { useState } from "react"
import { useSpecies, useCreateSpecies, useDeleteSpecies } from "../hooks/use-species"
import { SpeciesFormModal } from "./species-form-modal"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import type { CreateSpeciesDto, UpdateSpeciesDto } from "../types/species.types"

export function SpeciesTable() {
  const { data: species, isLoading } = useSpecies()
  const createSpecies = useCreateSpecies()
  const deleteSpecies = useDeleteSpecies()

  const [modalOpen, setModalOpen] = useState(false)

  async function handleSave(dto: CreateSpeciesDto | UpdateSpeciesDto) {
    await createSpecies.mutateAsync(dto as CreateSpeciesDto)
    setModalOpen(false)
}

  async function handleDelete(id: number) {
    if (!confirm("¿Estás seguro de eliminar esta especie?")) return
    await deleteSpecies.mutateAsync(id)
  }

  if (isLoading) return <p className="p-4">Cargando especies...</p>

  return (
    <div className="p-6 space-y-4">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Especies</h1>
        <Button onClick={() => setModalOpen(true)} className="bg-blue-400 hover:bg-blue-700 text-white">+ Nueva Especie</Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Lista de Especies</CardTitle>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>ID</TableHead>
                <TableHead>Nombre</TableHead>
                <TableHead className="text-right">Acciones</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {species && species.length > 0 ? (
                species.map((s) => (
                  <TableRow key={s.id}>
                    <TableCell>{s.id}</TableCell>
                    <TableCell>{s.speciesName}</TableCell>
                    <TableCell className="text-right">
                      <Button
                        variant="destructive"
                        size="sm"
                        onClick={() => handleDelete(s.id)}
                        disabled={deleteSpecies.isPending}
                      >
                        Eliminar
                      </Button>
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={3} className="text-center text-muted-foreground">
                    No hay especies registradas
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

      <SpeciesFormModal
        open={modalOpen}
        onOpenChange={setModalOpen}
        isSaving={createSpecies.isPending}
        onSave={handleSave}
      />
    </div>
  )
}