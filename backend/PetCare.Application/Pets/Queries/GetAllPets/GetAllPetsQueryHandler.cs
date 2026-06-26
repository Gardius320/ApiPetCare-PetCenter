using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Pets.Queries.GetAllPets;


public class PaginatedPetsResult
{
    public List<GetPetDTO> Items { get; set; } = new();
    public int TotalRecords { get; set; }
}

public class GetAllPetsQueryHandler : IRequestHandler<GetAllPetsQuery, PaginatedPetsResult>
{
    private readonly IPetRepository _repository;

    public GetAllPetsQueryHandler(IPetRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedPetsResult> Handle(GetAllPetsQuery request, CancellationToken ct)
    {
        
        var (pets, totalRecords) = await _repository.GetAllPagesAsync(
            request.Page, request.PageSize, request.Search);

       
        var items = new List<GetPetDTO>();
        foreach (var pet in pets)
        {
            var dto = new GetPetDTO
            {
                Id               = pet.Id,
                Nombre           = pet.PetName,
                Especie          = pet.Specie?.SpecieName ?? string.Empty,
                PropietarioId    = pet.OwnerId,
                Propietario      = pet.Owner?.OwnerName ?? string.Empty,
                EmailPropietario = pet.Owner?.Email ?? string.Empty,
               
                Estado           = pet.IsActive ? "Activo" : "Inactivo"
            };
            items.Add(dto);
        }

        return new PaginatedPetsResult
        {
            Items        = items,
            TotalRecords = totalRecords
        };
    }
}
