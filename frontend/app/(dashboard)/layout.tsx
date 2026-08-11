import { Sidebar } from "@/components/sidebar/sidebar"
import AuthGuard from "@/components/auth/auth-guard"

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <AuthGuard>
      <div className="flex h-screen overflow-hidden">
        <div className="no-print contents">
          <Sidebar />
        </div>
        <main className="flex-1 overflow-y-auto bg-background p-6">
          {children}
        </main>
      </div>
    </AuthGuard>
  )
}