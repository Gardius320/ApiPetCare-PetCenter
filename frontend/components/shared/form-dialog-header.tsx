import type { LucideIcon } from "lucide-react"
import { DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { cn } from "@/lib/utils"

interface FormDialogHeaderProps {
  icon: LucideIcon
  title: string
  accent?: string
  className?: string
}

export function FormDialogHeader({ icon: Icon, title, accent = "#1F6F5C", className }: FormDialogHeaderProps) {
  return (
    <DialogHeader className={cn("flex-row items-center gap-3", className)}>
      <div
        className="flex size-9 shrink-0 items-center justify-center rounded-lg"
        style={{ backgroundColor: accent }}
      >
        <Icon className="size-[18px] text-white" />
      </div>
      <DialogTitle className="font-heading font-medium text-base text-foreground">
        {title}
      </DialogTitle>
    </DialogHeader>
  )
}