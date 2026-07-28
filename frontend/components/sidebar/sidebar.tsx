"use client";

import { useState } from "react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { LayoutDashboard, PawPrint, Users, Calendar, Cat, UserCog, ChevronLeft, ChevronRight, Package, LogOut,} from "lucide-react";
import { Tooltip, TooltipContent, TooltipTrigger,} from "@/components/ui/tooltip";
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
  { label: "Usuarios",     href: "/users",         icon: UserCog },
];

export function Sidebar() {
  const [collapsed, setCollapsed] = useState(false);
  const pathname = usePathname();
  const { logout } = useAuth();

  const logoutButton = (
    <button
      onClick={() => logout()}
      className={cn(
        "flex items-center gap-3 px-2.5 py-2 rounded-[8px] text-sm font-medium w-full",
        "transition-colors duration-100",
        collapsed && "justify-center px-0",
        "text-[#B5443A] hover:bg-[#B5443A]/10"
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
        <div className="flex items-center justify-center w-[26px] h-[26px] rounded-lg bg-[#1F6F5C] shrink-0">
          <PawPrint className="size-4 text-white" />
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
                  ? "bg-[#1F6F5C] text-white"
                  : "text-[#5B6B66] hover:bg-[#1F6F5C]/10 hover:text-[#16302B]"
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

      {/* Cerrar sesión */}
      <div className="border-t border-[#16302B]/10 p-2">
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