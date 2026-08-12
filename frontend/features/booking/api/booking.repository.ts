import api from "@/lib/axios"
import type { BookOnlineDto } from "../types/booking.types"
import type { ApiResponse } from "@/lib/types/api-response.types"

const BASE_URL = "/Appointment"

export const bookingRepository = {
  getAvailableSlots: (date: string): Promise<string[] | null> =>
  api.get<ApiResponse<string[]>>(`${BASE_URL}/AvailableSlots`, { params: { date } })
    .then((res) => res.data.data),

bookOnline: (dto: BookOnlineDto): Promise<number | null> =>
  api.post<ApiResponse<number>>(`${BASE_URL}/BookOnline`, dto)
    .then((res) => res.data.data),
}