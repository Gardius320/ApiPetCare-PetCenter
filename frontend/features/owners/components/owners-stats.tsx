"use client"

import { Users, UserCheck } from "lucide-react"
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  CardAction,
} from "@/components/ui/card"
import { Skeleton } from "@/components/ui/skeleton"
import { cn } from "@/lib/utils"
import { ownerService } from "../api/owner.service"
import type { Owner, OwnerStats } from "../types/owner.types"

interface OwnersStatsProps {
  owners: Owner[]
  isLoading: boolean
}

const STATS: {
  key: keyof OwnerStats
  label: string
  icon: React.ElementType
  iconColor: string
  iconBg: string
}[] = [
  {
    key: "total",
    label: "Total Propietarios",
    icon: Users,
    iconColor: "text-primary",
    iconBg: "bg-accent",
  },
  {
    key: "male",
    label: "Hombres",
    icon: UserCheck,
    iconColor: "text-[#1F6F5C]",
    iconBg: "bg-accent",
  },
  {
    key: "female",
    label: "Mujeres",
    icon: UserCheck,
    iconColor: "text-[#FF8C6B]",
    iconBg: "bg-secondary",
  },
]

export function OwnersStats({ owners, isLoading }: OwnersStatsProps) {
  const stats = ownerService.computeStats(owners)

  return (
    <div className="grid grid-cols-2 gap-4 lg:grid-cols-4">
      {STATS.map(({ key, label, icon: Icon, iconColor, iconBg }) => (
        <Card key={key}>
          <CardHeader>
            <CardTitle className="text-sm font-medium text-muted-foreground">
              {label}
            </CardTitle>
            <CardAction>
              <div
                className={cn(
                  "flex size-9 items-center justify-center rounded-lg",
                  iconBg
                )}
              >
                <Icon className={cn("size-5", iconColor)} />
              </div>
            </CardAction>
          </CardHeader>
          <CardContent>
            {isLoading ? (
              <Skeleton className="h-8 w-16" />
            ) : (
              <p className="text-3xl font-bold text-foreground">{stats[key]}</p>
            )}
          </CardContent>
        </Card>
      ))}
    </div>
  )
}
