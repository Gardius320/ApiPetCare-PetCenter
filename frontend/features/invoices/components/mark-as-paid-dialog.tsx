"use client"

import { useState } from "react"
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { useMarkAsPaid } from "../hooks/use-invoices"
import { PaymentMethod, PAYMENT_METHOD_LABELS } from "../types/invoice.types"

interface MarkAsPaidDialogProps {
  invoiceId: string | null
  invoiceNumber?: string
  open: boolean
  onOpenChange: (open: boolean) => void
}

export function MarkAsPaidDialog({ invoiceId, invoiceNumber, open, onOpenChange }: MarkAsPaidDialogProps) {
  const [paymentMethod, setPaymentMethod] = useState<string>(String(PaymentMethod.Cash))
  const [paymentReference, setPaymentReference] = useState("")

  const markAsPaid = useMarkAsPaid()

  const handleConfirm = () => {
    if (!invoiceId) return

    markAsPaid.mutate(
      {
        id: invoiceId,
        dto: {
          paymentMethod: Number(paymentMethod) as PaymentMethod,
          paymentReference: paymentReference.trim() || null,
        },
      },
      {
        onSuccess: () => {
          onOpenChange(false)
          setPaymentMethod(String(PaymentMethod.Cash))
          setPaymentReference("")
        },
      }
    )
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Marcar como pagada{invoiceNumber ? ` — ${invoiceNumber}` : ""}</DialogTitle>
        </DialogHeader>

        <div className="space-y-4 py-2">
          <div className="flex flex-col gap-1.5">
            <Label>Método de pago</Label>
            <Select value={paymentMethod} onValueChange={setPaymentMethod}>
              <SelectTrigger className="w-full">
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                {Object.entries(PAYMENT_METHOD_LABELS).map(([value, label]) => (
                  <SelectItem key={value} value={value}>
                    {label}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <div className="flex flex-col gap-1.5">
            <Label>Referencia de pago (opcional)</Label>
            <Input
              value={paymentReference}
              onChange={(e) => setPaymentReference(e.target.value)}
              placeholder="Ej. número de transacción"
            />
          </div>
        </div>

        <DialogFooter>
          <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
            Cancelar
          </Button>
          <Button
            type="button"
            onClick={handleConfirm}
            disabled={markAsPaid.isPending}
            className="bg-primary text-primary-foreground hover:bg-primary/90"
          >
            {markAsPaid.isPending ? "Guardando..." : "Confirmar pago"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}