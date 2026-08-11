import { serviceRepository } from './service.repository';
import type { CreateServiceDto, UpdateServiceDto } from '../types/service.types';

export const serviceService = {
  getAll: (page: number, pageSize: number, search: string) => {
    return serviceRepository.getAll(page, pageSize, search);
  },

  getById: (id: number) => {
    return serviceRepository.getById(id);
  },

  create: (dto: CreateServiceDto) => {
    return serviceRepository.create(dto);
  },
  update: (dto: UpdateServiceDto) => {
    return serviceRepository.update(dto);
  },
  delete: (id: number) => {
    return serviceRepository.delete(id);
  },
};
