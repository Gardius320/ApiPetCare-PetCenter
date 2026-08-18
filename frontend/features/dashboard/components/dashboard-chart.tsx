"use client"

import { BarChart, Bar, Cell, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer,} from "recharts"

interface DashboardChartProps {
  title: string
  data: { status: string; count: number }[]
}

const CHART_COLORS = ["#1F6F5C", "#FF8C6B", "#E0A458", "#5B6B66", "#16302B"]

export function DashboardChart({ title, data }: DashboardChartProps) {
  return (
    <div className="rounded-lg border border-border bg-card p-6 shadow-md">
      <h2 className="flex items-center gap-2 font-heading font-medium text-xl text-foreground mb-4">
        {/* eslint-disable-next-line @next/next/no-img-element */}
        <img
          src="/decorations/paw-print.svg"
          alt=""
          aria-hidden="true"
          className="w-[14px] h-[14px]"
        />
        {title}
      </h2>

      <ResponsiveContainer width="80%" height={150}>
        <BarChart data={data}>
          <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
          <XAxis dataKey="status" stroke="#6b7280" />
          <YAxis stroke="#6b7280" allowDecimals={false} />
          <Tooltip
            contentStyle={{ borderRadius: "8px", border: "1px solid #e5e7eb" }}
          />
          <Bar dataKey="count" radius={[8, 8, 0, 0]}>
            {data.map((entry, index) => (
              <Cell
                key={entry.status}
                fill={CHART_COLORS[index % CHART_COLORS.length]}
              />
            ))}
          </Bar>
        </BarChart>
      </ResponsiveContainer>
    </div>
  )
}
