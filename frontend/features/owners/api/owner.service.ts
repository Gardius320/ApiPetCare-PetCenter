import { ownerRepository } from "./owner.repository"
import type { Owner, CreateOwnerDto, UpdateOwnerDto, OwnerStats } from "../types/owner.types"

export const ownerService = {
  getAll: (page = 1, pageSize = 100, search = "") => ownerRepository.getAll(page, pageSize, search),
  create: (dto: CreateOwnerDto) => ownerRepository.create(dto),
  update: (id: number, dto: UpdateOwnerDto) => ownerRepository.update(id, dto),
  delete: (id: number) => ownerRepository.delete(id),

  computeStats(owners: Owner[]): OwnerStats {
    return {
      total: owners.length,
      hombres: owners.filter((o) => o.gender?.toLowerCase() === "masculino").length,
      mujeres: owners.filter((o) => o.gender?.toLowerCase() === "femenino").length,
      // se asume que propietarios sin mascota activa son "inactivos" — placeholder
      inactivos: 0,
    }
  },
}
