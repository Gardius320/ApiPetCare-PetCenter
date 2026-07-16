import api from '@/lib/axios'
import type { Supply, CreateSupplyDto, UpdateSupplyDto, SupplyStats } from '../types/supply.types'
import type { ApiResponse } from '@/lib/types/api-response.types'
import type { PaginatedResult } from '@/lib/types/paginated-result.types'

export const supplyRepository = {
  getAll: (page = 1, pageSize = 10, search = '', categoryId?: number, onlyActive = true) =>
    api
      .get<ApiResponse<PaginatedResult<Supply>>>('/Supplies/GetAll', {
        params: { page, pageSize, search, categoryId, onlyActive },
      })
      .then((res) => res.data),

  getStats: () =>
    api.get<ApiResponse<SupplyStats>>('/Supplies/Stats').then((res) => res.data),

  create: (dto: CreateSupplyDto) =>
    api.post<ApiResponse<Supply>>('/Supplies/Crear', dto).then((res) => res.data),

  update: (id: number, dto: UpdateSupplyDto) =>
    api.put<ApiResponse<Supply>>(`/Supplies/Actualizar/${id}`, dto).then((res) => res.data),

  toggleStatus: (id: number) =>
    api.put<ApiResponse<Supply>>(`/Supplies/CambiarEstado/${id}`).then((res) => res.data),
}