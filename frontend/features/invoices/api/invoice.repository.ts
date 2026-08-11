import api from "@/lib/axios"
import type { ApiResponse } from "@/lib/types/api-response.types"
import type { CreateInvoiceRequest, Invoice, InvoiceDetail, InvoiceFilters, PayInvoiceRequest,} from "../types/invoice.types"


const BASE = "/Invoices"

export const invoiceRepository = {
  create(dto: CreateInvoiceRequest): Promise<ApiResponse<Invoice>> {
    return api.post<ApiResponse<Invoice>>(BASE, dto).then((res) => res.data)
  },

  getById(id: string): Promise<ApiResponse<InvoiceDetail>> {
    return api.get<ApiResponse<InvoiceDetail>>(`${BASE}/${id}`).then((res) => res.data)
  },

  getByOwner(ownerId: number): Promise<ApiResponse<Invoice[]>> {
    return api.get<ApiResponse<Invoice[]>>(`${BASE}/owner/${ownerId}`).then((res) => res.data)
  },

  getAll(filters?: InvoiceFilters): Promise<ApiResponse<Invoice[]>> {
    return api
      .get<ApiResponse<Invoice[]>>(BASE, {
        params: {
          from: filters?.from,
          to: filters?.to,
          status: filters?.status,
        },
      })
      .then((res) => res.data)
  },

  cancel(id: string): Promise<ApiResponse<null>> {
    return api.put<ApiResponse<null>>(`${BASE}/${id}/cancel`).then((res) => res.data)
  },
  pay(id: string, dto: PayInvoiceRequest): Promise<ApiResponse<null>> {
  return api.put<ApiResponse<null>>(`${BASE}/${id}/pay`, dto).then((res) => res.data)
},
}
