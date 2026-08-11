import { medicalRecordsRepository } from "./medical-record.repository";
import type {
  CreateMedicalRecordPayload,
  UpdateMedicalRecordPayload,
  MedicalRecordFilters,
} from "../types/medical-record.types";

export const medicalRecordsService = {
  create: async (payload: CreateMedicalRecordPayload) => {
    const { data } = await medicalRecordsRepository.create(payload);
    return data; // id del registro creado
  },

  getById: async (id: number) => {
    const { data } = await medicalRecordsRepository.getById(id);
    return data.data; // desenvuelve ApiResponse<T>
  },

  getByPetId: async (petId: number) => {
    const { data } = await medicalRecordsRepository.getByPetId(petId);
    return data;
  },

  getAll: async (filters?: MedicalRecordFilters) => {
    const { data } = await medicalRecordsRepository.getAll(filters);
    return data;
  },

  update: async (payload: UpdateMedicalRecordPayload) => {
    const { data } = await medicalRecordsRepository.update(payload);
    return data.data;
  },

  delete: async (id: number) => {
    const { data } = await medicalRecordsRepository.delete(id);
    return data;
  },
};