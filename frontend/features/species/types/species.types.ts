export interface Species {
    id: number
    speciesName: string
}

export interface CreateSpeciesDto {
    speciesName: string
}

export interface UpdateSpeciesDto {
    id: number
    speciesName: string
}

export interface SpeciesStats {
    total: number
}