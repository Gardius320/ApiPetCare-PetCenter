import api from "@/lib/axios"
import type { Owner, CreateOwnerDto, UpdateOwnerDto } from "../types/owner.types"
import type { ApiResponse } from "@/lib/types/api-response.types"

const BASE = "/Owners"

export const ownerRepository = {
  getAll(page = 1, pageSize = 100, search = ""): Promise<{ items: Owner[]; total: number }> {
    return api.get(`${BASE}/GetAll`, { params: { page, pageSize, search } }).then((res) => ({
      items: res.data.data.items,
      total: res.data.data.totalRecords,
    }))
  },
  create(dto: CreateOwnerDto): Promise<Owner> {
    return api.post<Owner>(`${BASE}/Create`, dto).then((res) => res.data)
  },
  update(id: number, dto: UpdateOwnerDto): Promise<Owner> {
    return api.put<Owner>(`${BASE}/Update/${id}`, dto).then((res) => res.data)
  },
  delete(id: number): Promise<ApiResponse<string>> {
    return api.delete<ApiResponse<string>>(`${BASE}/Delete/${id}`).then((res) => res.data)
  },
}