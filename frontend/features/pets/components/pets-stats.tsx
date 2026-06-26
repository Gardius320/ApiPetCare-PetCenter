import { PawPrint, Heart, Stethoscope, Archive } from "lucide-react"
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  CardAction,
} from "@/components/ui/card"
import { Skeleton } from "@/components/ui/skeleton"
import { cn } from "@/lib/utils"
import { petService } from "../api/pet.service"
import type { Pet, PetStats } from "../types/pet.types"

interface PetsStatsProps {
  pets: Pet[]
  isLoading: boolean
}

const STATS: {
  key: keyof PetStats
  label: string
  icon: React.ElementType
  iconColor: string
  iconBg: string
}[] = [
  {
    key: "total",
    label: "Total Mascotas",
    icon: PawPrint,
    iconColor: "text-blue-600",
    iconBg: "bg-blue-50",
  },
  {
    key: "activos",
    label: "Activas",
    icon: Heart,
    iconColor: "text-green-600",
    iconBg: "bg-green-50",
  },
  {
    key: "enTratamiento",
    label: "En Tratamiento",
    icon: Stethoscope,
    iconColor: "text-amber-600",
    iconBg: "bg-amber-50",
  },
  {
    key: "inactivos",
    label: "Inactivas",
    icon: Archive,
    iconColor: "text-gray-500",
    iconBg: "bg-gray-100",
  },
]

export function PetsStats({ pets, isLoading }: PetsStatsProps) {
  const stats = petService.computeStats(pets)

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
