"use client"

import { useMedicalRecordsByPetId, useDeleteMedicalRecord } from "../hooks/use-medical-record"
import type { MedicalRecord } from "../types/medical-record.types"
import { Pencil, Trash2, NotebookPen} from "lucide-react"

interface MedicalRecordTableProps {
  petId: number
  onCreateClick: () => void
  onEditClick: (record: MedicalRecord) => void
}

export function MedicalRecordTable({ petId, onCreateClick, onEditClick }: MedicalRecordTableProps) {
  const { data: records, isLoading, isError } = useMedicalRecordsByPetId(petId)
  const deleteMutation = useDeleteMedicalRecord()

  const handleDelete = async (id: number) => {
    if (!confirm("¿Eliminar esta entrada del historial clínico?")) return
    await deleteMutation.mutateAsync(id)
  }

  if (isLoading) return <div>Cargando historial clínico...</div>
  if (isError) return <div>Error al cargar el historial clínico.</div>

  return (
    <div className="medical-record-timeline">
  <div className="timeline-header flex items-center justify-between">
    <h3>Historial Clínico</h3><br />
    <br></br>
    
  </div>

  <div className="timeline-actions flex gap-3">
    <button
      onClick={onCreateClick}
      className="flex items-center gap-1 bg-primary text-primary-foreground text-xs font-semibold px-3 py-1.5 rounded-full hover:opacity-90 transition" >
      <NotebookPen className="h-4 w-4" />
      Nuevo Historial
    </button>
    </div>
        
      
      {records && records.length === 0 && (
        <p>Esta mascota no tiene entradas en su historial clínico todavía.</p>
      )}

      <ul className="timeline-list">
        {records?.map((record) => (
          <li key={record.id} className="timeline-item">
            <div className="timeline-date">
              {new Date(record.visitDate).toLocaleDateString()}
            </div>
            <div className="timeline-content">
              <p><strong>Diagnóstico:</strong> {record.diagnosis}</p>
              <p><strong>Tratamiento:</strong> {record.treatment}</p>
              <p><strong>Veterinario:</strong> {record.veterinarianName}</p>
              {record.weight != null && <p><strong>Peso:</strong> {record.weight} kg</p>}
              {record.temperature != null && <p><strong>Temperatura:</strong> {record.temperature} °C</p>}
              {record.observations && <p><strong>Observaciones:</strong> {record.observations}</p>}
              {record.nextFollowUpDate && (
                <p><strong>Próximo control:</strong> {new Date(record.nextFollowUpDate).toLocaleDateString()}</p>
              )}
            </div>
            <div className="timeline-actions flex gap-3">
             <button onClick={() => onEditClick(record)}
             className="flex items-center gap-1 bg-secondary text-secondary-foreground text-xs font-semibold px-3 py-1.5 rounded-full hover:opacity-90 transition" >
              <Pencil className="h-3 w-3" /> Editar</button>
              <button onClick={() => handleDelete(record.id)}
               className="flex items-center gap-1 bg-destructive/10 text-destructive text-xs font-semibold px-3 py-1.5 rounded-full hover:bg-destructive/20 transition" >
               <Trash2 className="h-3 w-3" /> Eliminar</button>
              </div>
          </li>
        ))}
      </ul>
    </div>
  )
}