import { InvoiceTable } from "@/features/invoices/components/invoice-table"

export default function InvoicesPage() {
  return (
    <div className="space-y-2">
      <h1 className="text-2xl font-bold text-foreground">Facturación</h1>
      <p className="text-sm text-muted-foreground">Gestión de facturas.</p>
      <InvoiceTable />
    </div>
  )
}
