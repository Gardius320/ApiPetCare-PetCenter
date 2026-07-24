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
                Name             = pet.PetName,
                Species          = pet.Specie?.SpecieName ?? string.Empty,
                OwnerId          = pet.OwnerId,
                OwnerName        = pet.Owner?.OwnerName ?? string.Empty,
                EmailOwner       = pet.Owner?.Email ?? string.Empty,

                IsActive        = pet.IsActive
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
