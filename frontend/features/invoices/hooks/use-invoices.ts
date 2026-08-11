import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { toast } from "sonner"
import { invoiceService } from "../api/invoice.service"
import type { CreateInvoiceRequest, InvoiceFilters, PayInvoiceRequest } from "../types/invoice.types"
import { getErrorMessage } from "@/lib/get-error-message"

const QUERY_KEY = ["invoices"] as const

export const useInvoices = (filters?: InvoiceFilters) => {
  return useQuery({
    queryKey: [...QUERY_KEY, filters ?? {}],
    queryFn: () => invoiceService.getAll(filters),
  })
}

export const useInvoice = (id: string | undefined) => {
  return useQuery({
    queryKey: [...QUERY_KEY, id],
    queryFn: () => invoiceService.getById(id as string),
    enabled: !!id,
  })
}

export const useInvoicesByOwner = (ownerId: number | undefined) => {
  return useQuery({
    queryKey: [...QUERY_KEY, "owner", ownerId],
    queryFn: () => invoiceService.getByOwner(ownerId as number),
    enabled: !!ownerId,
  })
}

export const useCreateInvoice = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (dto: CreateInvoiceRequest) => invoiceService.create(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: QUERY_KEY })
      toast.success("Factura creada correctamente")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo crear la factura"))
    },
  })
}

export const useCancelInvoice = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (id: string) => invoiceService.cancel(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: QUERY_KEY })
      toast.success("Factura anulada correctamente")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo anular la factura"))
    },
  })
}
export const useMarkAsPaid = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ({ id, dto }: { id: string; dto: PayInvoiceRequest }) => invoiceService.pay(id, dto),
    onSuccess: (_data, variables) => {
      queryClient.invalidateQueries({ queryKey: QUERY_KEY })
      queryClient.invalidateQueries({ queryKey: [...QUERY_KEY, variables.id] })
      toast.success("Factura marcada como pagada")
    },
    onError: (err) => {
      toast.error(getErrorMessage(err, "No se pudo marcar la factura como pagada"))
    },
  })
}

