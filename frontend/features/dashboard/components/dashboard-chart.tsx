"use client"

import {
  BarChart,
  Bar,
  Cell,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts"

interface DashboardChartProps {
  data: { status: string; count: number }[]
}

const STATUS_COLOR: Record<string, string> = {
  Activos: "#1F6F5C",
  Inactivos: "#E0A458",
}

export function DashboardChart({ data }: DashboardChartProps) {
  return (
    <div className="rounded-2xl border border-gray-200 bg-white p-6 shadow-md font-[family-name:var(--font-inter)]">
      <h2 className="flex items-center gap-2 font-[family-name:var(--font-space-grotesk)] font-medium text-xl text-[#16302B] mb-4">
        {/* eslint-disable-next-line @next/next/no-img-element */}
        <img
          src="/decorations/paw-print.svg"
          alt=""
          aria-hidden="true"
          className="w-[14px] h-[14px]"
        />
        Mascotas por estado
      </h2>

      <ResponsiveContainer width="100%" height={300}>
        <BarChart data={data}>
          <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
          <XAxis dataKey="status" stroke="#6b7280" />
          <YAxis stroke="#6b7280" allowDecimals={false} />
          <Tooltip
            contentStyle={{ borderRadius: "8px", border: "1px solid #e5e7eb" }}
          />
          <Bar dataKey="count" radius={[8, 8, 0, 0]}>
            {data.map((entry) => (
              <Cell
                key={entry.status}
                fill={STATUS_COLOR[entry.status] ?? "#1F6F5C"}
              />
            ))}
          </Bar>
        </BarChart>
      </ResponsiveContainer>
    </div>
  )
}
