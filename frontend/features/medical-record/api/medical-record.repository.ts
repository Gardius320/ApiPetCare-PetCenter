import api from "@/lib/axios"
import type {
  MedicalRecord,
  CreateMedicalRecordPayload,
  UpdateMedicalRecordPayload,
  MedicalRecordFilters,
} from "../types/medical-record.types";
import type { ApiResponse } from "@/lib/types/api-response.types";

const BASE_URL = "/medicalrecords";

export const medicalRecordsRepository = {
  create: (payload: CreateMedicalRecordPayload) =>
    api.post<number>(BASE_URL, payload),

  getById: (id: number) =>
    api.get<ApiResponse<MedicalRecord>>(`${BASE_URL}/${id}`),

  getByPetId: (petId: number) =>
    api.get<MedicalRecord[]>(`${BASE_URL}/pet/${petId}`),

  getAll: (filters?: MedicalRecordFilters) =>
    api.get<MedicalRecord[]>(BASE_URL, { params: filters }),

  update: (payload: UpdateMedicalRecordPayload) =>
    api.put<ApiResponse<MedicalRecord>>(`${BASE_URL}/${payload.id}`, payload),

  delete: (id: number) =>
    api.delete<ApiResponse<string>>(`${BASE_URL}/${id}`),
};