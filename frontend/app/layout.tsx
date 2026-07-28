import type { Metadata } from "next"
import { Geist, Geist_Mono, Space_Grotesk, Inter, IBM_Plex_Mono } from "next/font/google"
import "./globals.css"
import { TooltipProvider } from "@/components/ui/tooltip"
import { ReactQueryProvider } from "@/components/providers/query-provider"
import { Toaster } from "@/components/ui/sonner"

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
})

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
})

// Cargadas para el rediseño del Dashboard (ver frontend/app/(dashboard)/dashboard y components/sidebar).
const spaceGrotesk = Space_Grotesk({
  variable: "--font-space-grotesk",
  subsets: ["latin"],
  weight: ["500"],
})

const inter = Inter({
  variable: "--font-inter",
  subsets: ["latin"],
})

const ibmPlexMono = IBM_Plex_Mono({
  variable: "--font-ibm-plex-mono",
  subsets: ["latin"],
  weight: ["500"],
})

export const metadata: Metadata = {
  title: "PetCare — Sistema Veterinario",
  description: "Gestión integral de clínica veterinaria",
}

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode
}>) {
  return (
    <html
      lang="es"
      className={`${geistSans.variable} ${geistMono.variable} ${spaceGrotesk.variable} ${inter.variable} ${ibmPlexMono.variable} h-full antialiased`}
    >
      <body className="min-h-full flex flex-col">
        <ReactQueryProvider>
          <TooltipProvider delayDuration={200}>{children}</TooltipProvider>
        </ReactQueryProvider>
        <Toaster richColors position="top-right" />
      </body>
    </html>
  )
}