import {PawPrint, Users, CalendarCheck } from "lucide-react";

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
            {label: "Total Mascotas", value: totalPets, icon : PawPrint, color: "text-blue-600 bg-blue-50"},
            {label: "Total Propietarios", value: totalOwners, icon: Users, color : "text-green-600 bg-green-50"},
            {label: "Total Citas", value: totalAppointments, icon: CalendarCheck, color : "text-purple-600 bg-purple-50"},
        ]
        return (
    <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
      {cards.map(({ label, value, icon: Icon, color }) => (
        <div
          key={label}
          className="rounded-2xl border border-gray-200 bg-white p-6 shadow-md flex items-center gap-4"
        >
          <div className={`rounded-full p-3 ${color}`}>
            <Icon className="h-6 w-6" />
          </div>
          <div>
            <p className="text-sm text-gray-500">{label}</p>
            <p className="text-2xl font-bold text-gray-800">{value}</p>
          </div>
        </div>
      ))}
    </div>
  )
      }