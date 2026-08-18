import { InvoiceStatus } from "../types/invoice.types"

export const INVOICE_STATUS_LABELS: Record<InvoiceStatus, string> = {
  [InvoiceStatus.Pending]: "Pendiente",
  [InvoiceStatus.Paid]: "Pagada",
  [InvoiceStatus.Cancelled]: "Anulada",
}

const STATUS_COLORS: Record<InvoiceStatus, string> = {
  [InvoiceStatus.Pending]: "bg-yellow-100 text-yellow-700",
  [InvoiceStatus.Paid]: "bg-green-100 text-green-700",
  [InvoiceStatus.Cancelled]: "bg-red-100 text-red-700",
}

export function InvoiceStatusBadge({ status }: { status: InvoiceStatus }) {
  return (
    <span
      className={`px-3 py-1 rounded-full text-xs font-semibold ${STATUS_COLORS[status] ?? "bg-muted text-muted-foreground"}`}
    >
      {INVOICE_STATUS_LABELS[status] ?? "Desconocido"}
    </span>
  )
}
