export enum InvoiceStatus {
  Pending = 0,
  Paid = 1,
  Cancelled = 2,
}

export enum InvoiceItemType {
  Service = 0,
  Supply = 1,
}

export interface Invoice {
  id: string
  invoiceNumber: string
  ownerId: number
  appointmentId: number | null
  issueDate: string
  status: InvoiceStatus
  subtotal: number
  tax: number
  total: number
  paymentMethod: PaymentMethod | null
  paymentReference: string | null
  paymentDate: string | null
}

export interface InvoiceItem {
  id: string // Guid
  itemType: InvoiceItemType
  description: string
  supplyId: number | null
  quantity: number
  unitPrice: number
  lineTotal: number
}

export interface InvoiceDetail {
  invoice: Invoice
  items: InvoiceItem[]
}

export interface CreateInvoiceItemRequest {
  itemType: InvoiceItemType
  description: string
  supplyId: number | null
  quantity: number
  unitPrice: number
}

export interface CreateInvoiceRequest {
  ownerId: number
  appointmentId: number | null
  items: CreateInvoiceItemRequest[]
}

export interface InvoiceFilters {
  from?: string
  to?: string
  status?: InvoiceStatus
}
export enum PaymentMethod {
  Cash = 0,
  Transfer = 1,
  Card = 2,
}

export const PAYMENT_METHOD_LABELS: Record<PaymentMethod, string> = {
  [PaymentMethod.Cash]: "Efectivo",
  [PaymentMethod.Transfer]: "Transferencia",
  [PaymentMethod.Card]: "Tarjeta",
}

export interface PayInvoiceRequest {
  paymentMethod: PaymentMethod
  paymentReference?: string | null
}