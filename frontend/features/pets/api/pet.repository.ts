import api from "@/lib/axios"
import type { Pet, CreatePetDto, UpdatePetDto } from "../types/pet.types"

const BASE = "/Pets"

interface RawPet {
  id: number
  nombre: string
  especie: string
  propietario: string
  emailPropietario: string
  estado: string
  especieId: number
  propietarioId: number
}

export const petRepository = {
  getAll(page = 1, pageSize = 10, search = ""): Promise<{ items: Pet[]; total: number }> {
    return api.get(`${BASE}/GetAll`, {
      params: { page, pageSize, search }
    }).then((res) => ({
      items: res.data.data.items.map((p: RawPet) => ({
        ...p,
        estadoId: p.estado === "Activo" ? 1 : 3,
        especieId: p.especieId ?? 0,
        propietarioId: p.propietarioId ?? 0,
      })),
      total: res.data.data.totalRecords,
    }))
  },

  create(dto: CreatePetDto): Promise<Pet> {
  return api.post<Pet>(`${BASE}/Crear`, {
    petName: dto.nombre,
    specieId: dto.especieId,
    ownerId: dto.propietarioId,
  }).then((res) => res.data)
},

  update(id: number, dto: UpdatePetDto): Promise<Pet> {
    return api.put<Pet>(`${BASE}/Actualizar/${id}`, dto).then((res) => res.data)
  },

  delete(id: number): Promise<void> {
    return api.delete(`${BASE}/Eliminar/${id}`).then(() => undefined)
  },

  changeState(id: number, isActive: boolean): Promise<void> {
    return api.patch(`${BASE}/CambiarEstado/${id}`, { petId: id, isActive }).then(() => undefined)
  },
}