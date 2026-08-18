import { Skeleton } from "@/components/ui/skeleton"

interface TableSkeletonProps {
  title?: string
  columns: number
  rows?: number
  columnWidths?: string[]
  showActions?: boolean
}

export function TableSkeleton({
  title = "Cargando...",
  columns,
  rows = 6,
  columnWidths,
  showActions = true,
}: TableSkeletonProps) {
  return (
    <div className="rounded-lg border border-border bg-card p-6 shadow-md">

  {/* Header */}
<div className="flex items-center justify-between mb-4">
  <h2 className="text-xl font-bold text-foreground">{title}</h2>
  <Skeleton className="h-9 w-44 rounded-lg" />
</div>

      {/* Filas */}
      <table className="w-full">
        <tbody>
          {Array.from({ length: rows }).map((_, r) => (
            <tr key={r} className="border-b border-border">
              {Array.from({ length: columns }).map((_, c) => (
                <td key={c} className="py-4 px-4">
                  <Skeleton className={`h-4 ${columnWidths?.[c] ?? "w-24"}`} />
                </td>
              ))}
              {showActions && (
                <td className="py-4 px-4">
                  <div className="flex gap-2">
                    <Skeleton className="h-6 w-16 rounded-full" />
                    <Skeleton className="h-6 w-16 rounded-full" />
                  </div>
                </td>
              )}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}