import { InvoiceForm } from "@/features/invoices/components/invoice-form"

export default function NewInvoicePage() {
  return (
    <div className="space-y-2">
      <h1 className="text-2xl font-bold text-foreground">Nueva factura</h1>
      <p className="text-sm text-muted-foreground">Registra una nueva factura para un propietario.</p>
      <InvoiceForm />
    </div>
  )
}
