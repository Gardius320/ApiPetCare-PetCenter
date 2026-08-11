import { Suspense } from "react"
import { MedicalRecordsPageContent } from "@/features/medical-record/components/medical-records-page-content"

export default function MedicalRecordsPage() {
  return (
    <Suspense fallback={null}>
      <MedicalRecordsPageContent />
    </Suspense>
  )
}
