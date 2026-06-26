import { AppointmentTable } from "@/features/appointments/components/appointment-table"

export default function AppointmentsPage() {
  return (
    <div className="space-y-2">
      <h1 className="text-2xl font-bold text-foreground">Citas</h1>
      <p className="text-sm text-muted-foreground">Gestión de citas veterinarias.</p>
      <AppointmentTable />
    </div>
  )
}