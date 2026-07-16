// features/supply/api/supply.service.ts
import { supplyRepository } from './supply.repository'
import type { Supply, CreateSupplyDto, UpdateSupplyDto } from '../types/supply.types'

export const supplyService = {
  async getAll(page = 1, pageSize = 10, search = '', categoryId?: number, onlyActive = true) {
    const response = await supplyRepository.getAll(page, pageSize, search, categoryId, onlyActive)
    return response.data
  },

  async getStats() {
    const response = await supplyRepository.getStats()
    return response.data
  },

  create: (dto: CreateSupplyDto) => supplyRepository.create(dto),

  update: (id: number, dto: UpdateSupplyDto) => supplyRepository.update(id, dto),

  toggleStatus: (id: number) => supplyRepository.toggleStatus(id),
}