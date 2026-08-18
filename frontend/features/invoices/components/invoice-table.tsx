"use client"

import { useState } from "react"
import Link from "next/link"
import { Receipt, Plus, CircleDollarSign } from "lucide-react"
import { useInvoices } from "../hooks/use-invoices"
import { useAllOwners } from "@/features/owners/hooks/use-owners"
import { InvoiceStatusBadge, INVOICE_STATUS_LABELS } from "./invoice-status-badge"
import { InvoiceStatus } from "../types/invoice.types"
import { TableSkeleton } from "@/components/shared/table-skeleton"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { MarkAsPaidDialog } from "./mark-as-paid-dialog"

const ALL_STATUSES = "all"

export function InvoiceTable() {
  const [status, setStatus] = useState<string>(ALL_STATUSES)
  const [from, setFrom] = useState("")
  const [to, setTo] = useState("")
  const [payDialogInvoice, setPayDialogInvoice] = useState<{ id: string; invoiceNumber: string } | null>(null)

  const { data: invoices, isLoading } = useInvoices({
    status: status === ALL_STATUSES ? undefined : (Number(status) as InvoiceStatus),
    from: from || undefined,
    to: to || undefined,
  })
  const { data: ownersData } = useAllOwners()

  const owners = ownersData?.items ?? []
  const ownerName = (ownerId: number) =>
    owners.find((o) => o.id === ownerId)?.ownerName ?? `#${ownerId}`

  if (isLoading) {
    return (
      <TableSkeleton
        title="Lista de Facturas"
        columns={6}
        columnWidths={["w-24", "w-32", "w-28", "w-20", "w-24", "w-28"]}
      />
    )
  }

  const list = invoices ?? []

  return (
    <div className="rounded-lg border border-border bg-card p-6 shadow-md">
      {/* Header */}
      <div className="flex items-center justify-between mb-4">
        <div className="flex items-center gap-2">
          <Receipt className="h-5 w-5 text-primary" />
          <h2 className="text-xl font-bold text-foreground">Lista de Facturas</h2>
        </div>
        <Link
          href="/invoices/new"
          className="flex items-center gap-2 bg-primary text-primary-foreground px-4 py-2 rounded-lg text-sm hover:opacity-90 transition"
        >
          <Plus className="h-4 w-4" />
          Nueva factura
        </Link>
      </div>

      {/* Filtros */}
      <div className="flex flex-wrap items-end gap-4 mb-4">
        <div className="flex flex-col gap-1.5">
          <label className="text-sm text-muted-foreground">Estado</label>
          <Select value={status} onValueChange={setStatus}>
            <SelectTrigger className="w-[160px]">
              <SelectValue />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value={ALL_STATUSES}>Todos</SelectItem>
              {Object.entries(INVOICE_STATUS_LABELS).map(([value, label]) => (
                <SelectItem key={value} value={value}>
                  {label}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="flex flex-col gap-1.5">
          <label className="text-sm text-muted-foreground">Desde</label>
          <input
            type="date"
            value={from}
            onChange={(e) => setFrom(e.target.value)}
            className="h-9 rounded-md border border-border px-3 text-sm"
          />
        </div>

        <div className="flex flex-col gap-1.5">
          <label className="text-sm text-muted-foreground">Hasta</label>
          <input
            type="date"
            value={to}
            onChange={(e) => setTo(e.target.value)}
            className="h-9 rounded-md border border-border px-3 text-sm"
          />
        </div>
      </div>

      {/* Tabla */}
      <table className="w-full text-left">
        <thead>
          <tr className="bg-muted">
            <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">N° Factura</th>
            <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Dueño</th>
            <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Fecha</th>
            <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Total</th>
            <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Estado</th>
            <th className="border-b border-border py-3 px-4 text-sm text-muted-foreground">Acciones</th>
          </tr>
        </thead>
        <tbody>
          {list.length === 0 && (
            <tr>
              <td colSpan={6} className="py-16 text-center text-sm text-muted-foreground">
                <Receipt className="mx-auto mb-2 size-8 opacity-25" />
                No se encontraron facturas
              </td>
            </tr>
          )}

          {list.map((invoice) => (
            <tr key={invoice.id} className="border-b border-border hover:bg-accent transition-colors">
              <td className="py-4 px-4">
                <Link href={`/invoices/${invoice.id}`} className="text-primary font-medium hover:underline">
                  {invoice.invoiceNumber}
                </Link>
              </td>
              <td className="py-4 px-4 text-muted-foreground">{ownerName(invoice.ownerId)}</td>
              <td className="py-4 px-4 text-muted-foreground">
                {new Date(invoice.issueDate).toLocaleDateString("es-CO")}
              </td>
              <td className="py-4 px-4 text-foreground font-medium font-mono">
                ${invoice.total.toLocaleString("es-CO", { minimumFractionDigits: 2 })}
              </td>
              <td className="py-4 px-4">
                <InvoiceStatusBadge status={invoice.status} />
              </td>
              <td className="py-4 px-4">
                {invoice.status === InvoiceStatus.Pending && (
                  <button
                    onClick={() => setPayDialogInvoice({ id: invoice.id, invoiceNumber: invoice.invoiceNumber })}
                    className="flex items-center gap-1 bg-emerald-100 text-emerald-800 text-xs font-semibold px-3 py-1.5 rounded-full hover:bg-emerald-200 transition"
                  >
                    <CircleDollarSign className="h-3 w-3" />
                    Marcar pagada
                  </button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <MarkAsPaidDialog
        invoiceId={payDialogInvoice?.id ?? null}
        invoiceNumber={payDialogInvoice?.invoiceNumber}
        open={!!payDialogInvoice}
        onOpenChange={(open) => !open && setPayDialogInvoice(null)}
      />
    </div>
  )
}