export interface MedicalRecord {
  id: number;
  petId: number;
  petName: string;
  appointmentId: number | null;
  veterinarianUserId: string;
  veterinarianName: string;
  visitDate: string; // ISO date string
  diagnosis: string;
  treatment: string;
  weight: number | null;
  temperature: number | null;
  observations: string | null;
  nextFollowUpDate: string | null;
}

export interface CreateMedicalRecordPayload {
  petId: number;
  appointmentId?: number | null;
  veterinarianUserId: string;
  visitDate: string;
  diagnosis: string;
  treatment: string;
  weight?: number | null;
  temperature?: number | null;
  observations?: string | null;
  nextFollowUpDate?: string | null;
}

export interface UpdateMedicalRecordPayload extends CreateMedicalRecordPayload {
  id: number;
}

export interface MedicalRecordFilters {
  from?: string;
  to?: string;
  vetId?: string;
}