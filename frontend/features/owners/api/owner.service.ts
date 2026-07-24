import { ownerRepository } from "./owner.repository"
import { GENDER, type Owner, type CreateOwnerDto, type UpdateOwnerDto, type OwnerStats } from "../types/owner.types"

export const ownerService = {
  getAll: (page = 1, pageSize = 100, search = "") => ownerRepository.getAll(page, pageSize, search),
  create: (dto: CreateOwnerDto) => ownerRepository.create(dto),
  update: (id: number, dto: UpdateOwnerDto) => ownerRepository.update(id, dto),
  delete: (id: number) => ownerRepository.delete(id),

  computeStats(owners: Owner[]): OwnerStats {
    return {
      total: owners.length,
      male: owners.filter((o) => o.gender?.toLowerCase() === GENDER.MALE.toLowerCase()).length,
      female: owners.filter((o) => o.gender?.toLowerCase() === GENDER.FEMALE.toLowerCase()).length,
    }
  },
}
