"use client"

import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts"

interface DashboardChartProps {
  data: { estado: string; cantidad: number }[]
}

export function DashboardChart({ data }: DashboardChartProps) {
  return (
    <div className="rounded-2xl border border-gray-200 bg-white p-6 shadow-md">
      <h2 className="text-xl font-bold text-gray-800 mb-4">
        Mascotas por estado
      </h2>

      <ResponsiveContainer width="100%" height={300}>
        <BarChart data={data}>
          <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
          <XAxis dataKey="estado" stroke="#6b7280" />
          <YAxis stroke="#6b7280" allowDecimals={false} />
          <Tooltip
            contentStyle={{ borderRadius: "8px", border: "1px solid #e5e7eb" }}
          />
          <Bar dataKey="cantidad" fill="#3b82f6" radius={[6, 6, 0, 0]} />
        </BarChart>
      </ResponsiveContainer>
    </div>
  )
}