import { PawPrint, Users, CalendarCheck } from "lucide-react";

interface DashboardStatsProps {
    totalPets: number
    totalOwners: number
    totalAppointments: number
}

export function DashboardStats({ totalPets,
     totalOwners,
      totalAppointments }:
      DashboardStatsProps) {
        const cards = [
            { label: "Total Mascotas", value: totalPets, icon: PawPrint, color: "#1F6F5C" },
            { label: "Total Propietarios", value: totalOwners, icon: Users, color: "#FF8C6B" },
            { label: "Total Citas", value: totalAppointments, icon: CalendarCheck, color: "#E0A458" },
        ]
        return (
    <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 font-[family-name:var(--font-inter)]">
      {cards.map(({ label, value, icon: Icon, color }) => (
        <div
          key={label}
          className="rounded-none border-l-4 bg-white p-5 shadow-[0_1px_3px_rgba(0,0,0,0.05)] flex flex-col gap-3"
          style={{ borderLeftColor: color }}
        >
          <div className="flex items-start justify-between">
            <p className="text-[12px] text-[#5B6B66]">{label}</p>
            <Icon className="size-5 shrink-0" style={{ color }} />
          </div>
          <p className="font-[family-name:var(--font-ibm-plex-mono)] font-medium text-[26px] leading-none text-[#16302B]">
            {value}
          </p>
          {label === "Total Citas" && value === 0 && (
            // TODO: reemplazar con ilustracion de Storyset para el estado vacio de "Total Citas"
            <div aria-hidden="true" />
          )}
        </div>
      ))}
    </div>
  )
      }
