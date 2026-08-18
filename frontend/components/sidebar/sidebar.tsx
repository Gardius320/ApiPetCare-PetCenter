"use client";

import { useState } from "react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { LayoutDashboard, PawPrint, Users, Calendar, Cat, UserCog, ChevronLeft, ChevronRight, Package, Wrench, Receipt, LogOut,} from "lucide-react";
import { Tooltip, TooltipContent, TooltipTrigger,} from "@/components/ui/tooltip";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { cn } from "@/lib/utils";
import { useAuth } from "@/features/auth/hooks/use-auth";

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
  { label: "Insumos",      href: "/Supply",        icon: Package },
  { label: "Servicios",    href: "/services",     icon: Wrench },
  { label: "Facturación",  href: "/invoices",      icon: Receipt },
  { label: "Usuarios",     href: "/users",         icon: UserCog },
];

export function Sidebar() {
  const [collapsed, setCollapsed] = useState(false);
  const pathname = usePathname();
  const { logout, user } = useAuth();

  const logoutButton = (
    <button
      onClick={() => logout()}
      className={cn(
        "flex items-center gap-3 px-2.5 py-2 rounded-[8px] text-sm font-medium w-full",
        "transition-colors duration-100",
        collapsed && "justify-center px-0",
        "text-destructive hover:bg-destructive/10"
      )}
    >
      <LogOut className="size-4 shrink-0" />
      {!collapsed && <span className="truncate">Cerrar sesión</span>}
    </button>
  );

  return (
    <aside
      className={cn(
        "relative flex flex-col h-screen bg-sidebar border-r border-sidebar-border",
        "font-[family-name:var(--font-inter)]",
        "transition-[width] duration-200 ease-in-out shrink-0",
        collapsed ? "w-14" : "w-60"
      )}
    >
      {/* Marca */}
      <div className="flex items-center h-16 px-3 border-b border-sidebar-border overflow-hidden">
        <div className="flex items-center justify-center w-[26px] h-[26px] rounded-lg bg-primary shrink-0">
          <PawPrint className="size-4 text-primary-foreground" />
        </div>
        {!collapsed && (
          <span className="ml-3 font-[family-name:var(--font-space-grotesk)] font-medium text-sidebar-foreground text-sm tracking-tight whitespace-nowrap">
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
                "flex items-center gap-3 px-2.5 py-2 rounded-[8px] text-sm font-medium",
                "transition-colors duration-100",
                collapsed && "justify-center px-0",
                isActive
                  ? "bg-primary text-primary-foreground"
                  : "text-muted-foreground hover:bg-primary/10 hover:text-foreground"
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

      {/* Decoracion */}
      {!collapsed && (
        <div className="flex justify-center py-2 px-2 overflow-hidden">
          {/* eslint-disable-next-line @next/next/no-img-element */}
          <img
            src="/decorations/leaf-sprig.svg"
            alt=""
            aria-hidden="true"
            className="w-[95px] opacity-55 select-none pointer-events-none"
          />
        </div>
      )}

      {/* Usuario */}
      {user && (
        <div className={cn("flex items-center gap-2 px-3 py-2", collapsed && "justify-center px-0")}>
          <Avatar className="size-8 shrink-0">
            <AvatarFallback className="bg-primary/10 text-primary text-xs font-semibold">
              {user.fullName.charAt(0).toUpperCase()}
            </AvatarFallback>
          </Avatar>
          {!collapsed && (
            <div className="min-w-0">
              <p className="text-sm font-medium text-sidebar-foreground truncate">
                {user.fullName}
              </p>
              <p className="text-xs text-muted-foreground truncate">{user.role}</p>
            </div>
          )}
        </div>
      )}

      {/* Cerrar sesión */}
      <div className="border-t border-sidebar-border p-2">
        {collapsed ? (
          <Tooltip>
            <TooltipTrigger asChild>{logoutButton}</TooltipTrigger>
            <TooltipContent side="right">Cerrar sesión</TooltipContent>
          </Tooltip>
        ) : (
          logoutButton
        )}
      </div>

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