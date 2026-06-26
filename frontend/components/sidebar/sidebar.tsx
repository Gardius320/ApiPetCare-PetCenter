"use client";

import { useState } from "react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { LayoutDashboard, PawPrint, Users, Calendar, Cat, UserCog, ChevronLeft, ChevronRight, Stethoscope,} from "lucide-react";
import { Tooltip, TooltipContent, TooltipTrigger,} from "@/components/ui/tooltip";
import { cn } from "@/lib/utils";

interface NavItem {
  label: string;
  href: string;
  icon: React.ElementType;
}

const navItems: NavItem[] = [
  { label: "Dashboard",    href: "/dashboard",    icon: LayoutDashboard },
  { label: "Mascotas",     href: "/pets",          icon: PawPrint },
  { label: "Propietarios", href: "/owners",        icon: Users },
  { label: "Citas",        href: "/appointments",  icon: Calendar },
  { label: "Especies",     href: "/species",       icon: Cat },  
  { label: "Usuarios",     href: "/users",         icon: UserCog },
];

export function Sidebar() {
  const [collapsed, setCollapsed] = useState(false);
  const pathname = usePathname();

  return (
    <aside
      className={cn(
        "relative flex flex-col h-screen bg-sidebar border-r border-sidebar-border",
        "transition-[width] duration-200 ease-in-out shrink-0",
        collapsed ? "w-14" : "w-60"
      )}
    >
      {/* Marca */}
      <div className="flex items-center h-16 px-3 border-b border-sidebar-border overflow-hidden">
        <div className="flex items-center justify-center size-8 rounded-lg bg-sidebar-primary shrink-0">
          <Stethoscope className="size-4 text-sidebar-primary-foreground" />
        </div>
        {!collapsed && (
          <span className="ml-3 font-semibold text-sidebar-foreground text-sm tracking-tight whitespace-nowrap">
            PetCare
          </span>
        )}
      </div>

      {/* navegacion */}
      <nav className="flex-1 overflow-y-auto overflow-x-hidden py-3 px-2 space-y-0.5">
        {navItems.map(({ label, href, icon: Icon }) => {
          const isActive =
            pathname === href || pathname.startsWith(href + "/");

          const linkEl = (
            <Link
              href={href}
              className={cn(
                "flex items-center gap-3 px-2.5 py-2 rounded-md text-sm font-medium",
                "transition-colors duration-100",
                collapsed && "justify-center px-0",
                isActive
                  ? "bg-sidebar-primary text-sidebar-primary-foreground"
                  : "text-sidebar-foreground hover:bg-sidebar-accent hover:text-sidebar-accent-foreground"
              )}
            >
              <Icon className="size-4 shrink-0" />
              {!collapsed && (
                <span className="truncate">{label}</span>
              )}
            </Link>
          );

          if (collapsed) {
            return (
              <Tooltip key={href}>
                <TooltipTrigger asChild>{linkEl}</TooltipTrigger>
                <TooltipContent side="right">{label}</TooltipContent>
              </Tooltip>
            );
          }

          return <div key={href}>{linkEl}</div>;
        })}
      </nav>

      {/* Alternador para contraer */}
      <div className="border-t border-sidebar-border p-2">
        <button
          onClick={() => setCollapsed((prev) => !prev)}
          aria-label={collapsed ? "Expandir menú" : "Colapsar menú"}
          className={cn(
            "flex items-center justify-center w-full h-8 rounded-md",
            "text-sidebar-foreground/50 hover:bg-sidebar-accent hover:text-sidebar-accent-foreground",
            "transition-colors duration-100"
          )}
        >
          {collapsed ? (
            <ChevronRight className="size-4" />
          ) : (
            <ChevronLeft className="size-4" />
          )}
        </button>
      </div>
    </aside>
  );
}
