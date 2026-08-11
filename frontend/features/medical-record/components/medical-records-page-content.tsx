"use client"

import { useState } from "react"
import { useRouter, useSearchParams } from "next/navigation"
import { ArrowLeft, Stethoscope } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { usePets } from "@/features/pets/hooks/use-pets"
import { MedicalRecordTable } from "./medical-record-table"
import { MedicalRecordFormFields } from "./medical-record-form-fields"
import { useCreateMedicalRecord, useUpdateMedicalRecord } from "../hooks/use-medical-record"
import type {
  MedicalRecord,
  CreateMedicalRecordPayload,
  UpdateMedicalRecordPayload,
} from "../types/medical-record.types"

export function MedicalRecordsPageContent() {
  const router = useRouter()
  const searchParams = useSearchParams()
  const petIdParam = searchParams.get("petId")
  const petId = petIdParam ? Number(petIdParam) : null

  const [view, setView] = useState<"list" | "form">("list")
  const [selectedRecord, setSelectedRecord] = useState<MedicalRecord | undefined>(undefined)

  const { data: petsData } = usePets(1, 100, "")
  const pet = petId ? petsData?.items.find((p) => p.id === petId) : undefined

  const createMutation = useCreateMedicalRecord()
  const updateMutation = useUpdateMedicalRecord()
  const isSaving = createMutation.isPending || updateMutation.isPending

  function handleCreateClick() {
    setSelectedRecord(undefined)
    setView("form")
  }

  function handleEditClick(record: MedicalRecord) {
    setSelectedRecord(record)
    setView("form")
  }

  function handleCancel() {
    setView("list")
  }

  function handleSave(dto: CreateMedicalRecordPayload | UpdateMedicalRecordPayload, id?: number) {
    if (id) {
      updateMutation.mutate(dto as UpdateMedicalRecordPayload, {
        onSuccess: () => setView("list"),
      })
    } else {
      createMutation.mutate(dto as CreateMedicalRecordPayload, {
        onSuccess: () => setView("list"),
      })
    }
  }

  if (!petId) {
    return (
      <div className="space-y-4">
        <h1 className="text-2xl font-bold text-foreground">Historial clínico</h1>
        <Card>
          <CardContent className="py-10 text-center text-sm text-muted-foreground">
            No se especificó ninguna mascota. Vuelve a la lista de mascotas y usa el botón &quot;Historial&quot;
            en la fila correspondiente.
          </CardContent>
        </Card>
        <Button variant="outline" onClick={() => router.push("/pets")}>
          <ArrowLeft className="h-4 w-4" />
          Volver a Mascotas
        </Button>
      </div>
    )
  }

  return (
    <div className="space-y-4">
      <div>
        <Button
          variant="ghost"
          size="sm"
          onClick={() => router.push("/pets")}
          className="-ml-2 mb-1 text-muted-foreground"
        >
          <ArrowLeft className="h-4 w-4" />
          Mascotas
        </Button>
        <div className="flex items-center gap-2">
          <Stethoscope className="h-5 w-5 text-primary" />
          <h1 className="text-2xl font-bold text-foreground">
            Historial clínico{pet ? ` — ${pet.name}` : ""}
          </h1>
        </div>
      </div>

      <Card>
        {/* MedicalRecordTable ya trae su propio encabezado "Historial Clínico" + botón
            "+ Nueva entrada"; solo agregamos CardHeader para la vista de formulario,
            que ya no trae encabezado propio (antes vivía en FormDialogHeader). */}
        {view === "form" && (
          <CardHeader>
            <CardTitle>
              {selectedRecord ? "Editar entrada de historial" : "Nueva entrada de historial clínico"}
            </CardTitle>
          </CardHeader>
        )}
        <CardContent>
          {view === "list" ? (
            <MedicalRecordTable petId={petId} onCreateClick={handleCreateClick} onEditClick={handleEditClick} />
          ) : (
            <MedicalRecordFormFields
              petId={petId}
              record={selectedRecord}
              isSaving={isSaving}
              onSave={handleSave}
              onCancel={handleCancel}
            />
          )}
        </CardContent>
      </Card>
    </div>
  )
}
