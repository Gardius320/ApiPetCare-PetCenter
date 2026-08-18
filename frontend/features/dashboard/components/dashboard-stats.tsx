import Link from "next/link";
import { PawPrint, Users, CalendarCheck } from "lucide-react";
import { cn } from "@/lib/utils";

interface DashboardStatsProps {
  totalPets: number
  totalOwners: number
  totalAppointments: number
}

export function DashboardStats({ totalPets, totalOwners, totalAppointments }: DashboardStatsProps) {
  const cards = [
    { label: "Total Mascotas", value: totalPets, icon: PawPrint, highlight: false, href: "/pets" },
    { label: "Total Propietarios", value: totalOwners, icon: Users, highlight: false, href: "/owners" },
    { label: "Total Citas", value: totalAppointments, icon: CalendarCheck, highlight: true, href: "/appointments" },
  ]

  return (
    <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
      {cards.map(({ label, value, icon: Icon, highlight, href }) => (
        <Link
          key={label}
          href={href}
          className={cn(
            "rounded-lg border p-5 flex flex-col gap-3 transition-colors",
            highlight
              ? "bg-primary border-primary hover:bg-primary/90"
              : "bg-card border-border hover:border-primary"
          )}
        >
          <div className="flex items-start justify-between">
            <p className={cn("text-xs", highlight ? "text-primary-foreground/70" : "text-muted-foreground")}>
              {label}
            </p>
            <Icon className={cn("size-5 shrink-0", highlight ? "text-primary-foreground" : "text-primary")} />
          </div>
          <p
            className={cn(
              "font-mono font-medium text-2xl leading-none",
              highlight ? "text-primary-foreground" : "text-foreground"
            )}
          >
            {value}
          </p>
          {label === "Total Citas" && value === 0 && (
            <p className={cn("text-xs", highlight ? "text-primary-foreground/70" : "text-muted-foreground")}>
              Aún no hay citas registradas
            </p>
          )}
        </Link>
      ))}
    </div>
  )
}
