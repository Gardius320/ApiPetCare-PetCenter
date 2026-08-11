import { invoiceRepository } from "./invoice.repository"
import type { CreateInvoiceRequest, InvoiceFilters, PayInvoiceRequest } from "../types/invoice.types"

export const invoiceService = {
  create: async (dto: CreateInvoiceRequest) => {
    const response = await invoiceRepository.create(dto)
    return response.data!
  },
  getById: async (id: string) => {
    const response = await invoiceRepository.getById(id)
    return response.data!
  },
  getByOwner: async (ownerId: number) => {
    const response = await invoiceRepository.getByOwner(ownerId)
    return response.data ?? []
  },
  getAll: async (filters?: InvoiceFilters) => {
    const response = await invoiceRepository.getAll(filters)
    return response.data ?? []
  },
  cancel: (id: string) => invoiceRepository.cancel(id), 
  pay: (id: string, dto: PayInvoiceRequest) => invoiceRepository.pay(id, dto),
}
