import api from "@/lib/axios"
import type { Owner, CreateOwnerDto, UpdateOwnerDto } from "../types/owner.types"

const BASE = "/Owners"

export const ownerRepository = {
 getAll(page = 1, pageSize = 100, search = ""): Promise<{ items: Owner[]; total: number }> {
  return api.get(`${BASE}/GetAll`, { params: { page, pageSize, search } }).then((res) => ({
    items: res.data.data.items,
    total: res.data.data.totalRecords,
  }))
},
  create(dto: CreateOwnerDto): Promise<Owner> {
    return api.post<Owner>(`${BASE}/Crear`, dto).then((res) => res.data)
  },
  update(id: number, dto: UpdateOwnerDto): Promise<Owner> {
    return api.put<Owner>(`${BASE}/Actualizar/${id}`, dto).then((res) => res.data)
  },
  delete(id: number): Promise<void> {
    return api.delete(`${BASE}/Eliminar/${id}`).then(() => undefined)
  },
}
