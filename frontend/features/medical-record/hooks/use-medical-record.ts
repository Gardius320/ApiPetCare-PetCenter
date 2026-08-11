// features/medical-record/hooks/use-medical-record.ts
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { medicalRecordsService } from "../api/medical-record.service";
import type {
  CreateMedicalRecordPayload,
  UpdateMedicalRecordPayload,
  MedicalRecordFilters,
} from "../types/medical-record.types";

const QUERY_KEY = "medical-records";

// Listado general con filtros (from/to/vetId)
export function useMedicalRecords(filters?: MedicalRecordFilters) {
  return useQuery({
    queryKey: [QUERY_KEY, "all", filters],
    queryFn: () => medicalRecordsService.getAll(filters),
  });
}

// Historial completo de una mascota puntual — el más importante
export function useMedicalRecordsByPetId(petId: number) {
  return useQuery({
    queryKey: [QUERY_KEY, "by-pet", petId],
    queryFn: () => medicalRecordsService.getByPetId(petId),
    enabled: !!petId, // no ejecuta la query si petId es 0/undefined
  });
}

// Un registro puntual (para precargar el formulario en modo edición)
export function useMedicalRecordById(id: number | null) {
  return useQuery({
    queryKey: [QUERY_KEY, "by-id", id],
    queryFn: () => medicalRecordsService.getById(id!),
    enabled: !!id, // solo corre si hay un id (modo edición, no creación)
  });
}

export function useCreateMedicalRecord() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: CreateMedicalRecordPayload) =>
      medicalRecordsService.create(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: [QUERY_KEY, "all"] });
      queryClient.invalidateQueries({
        queryKey: [QUERY_KEY, "by-pet", variables.petId],
      });
    },
  });
}

export function useUpdateMedicalRecord() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: UpdateMedicalRecordPayload) =>
      medicalRecordsService.update(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: [QUERY_KEY, "all"] });
      queryClient.invalidateQueries({
        queryKey: [QUERY_KEY, "by-pet", variables.petId],
      });
      queryClient.invalidateQueries({
        queryKey: [QUERY_KEY, "by-id", variables.id],
      });
    },
  });
}

export function useDeleteMedicalRecord() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: number) => medicalRecordsService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [QUERY_KEY] });
    },
  });
}