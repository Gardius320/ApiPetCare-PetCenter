import api from "@/lib/axios"
import type { Pet, CreatePetDto, UpdatePetDto } from "../types/pet.types"

const BASE = "/Pets"

interface RawPet {
  id: number
  name: string
  species: string
  ownerId: number
  ownerName: string
  emailOwner: string
  isActive: boolean
}

export const petRepository = {
  getAll(page = 1, pageSize = 10, search = ""): Promise<{ items: Pet[]; total: number }> {
    return api.get(`${BASE}/GetAll`, {
      params: { page, pageSize, search }
    }).then((res) => ({
      items: res.data.data.items.map((p: RawPet) => ({
        id: p.id,
        name: p.name,
        species: p.species,
        ownerId: p.ownerId,
        ownerName: p.ownerName,
        emailOwner: p.emailOwner,
        isActive: p.isActive,
      })),
      total: res.data.data.totalRecords,
    }))
  },

  create(dto: CreatePetDto): Promise<Pet> {
    return api.post<Pet>(`${BASE}/Create`, dto).then((res) => res.data)
  },

  update(id: number, dto: UpdatePetDto): Promise<Pet> {
    return api.put<Pet>(`${BASE}/Update/${id}`, dto).then((res) => res.data)
  },

  changeState(id: number, isActive: boolean): Promise<void> {
    return api.patch(`${BASE}/ChangeState/${id}`, { petId: id, isActive }).then(() => undefined)
  },
}
