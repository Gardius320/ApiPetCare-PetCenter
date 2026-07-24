using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Owners.Queries.GetAllOwners
{
    
    public class PaginatedOwnersResult
    {
        public List<OwnerDTO> Items { get; set; } = new();
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }

    public class GetAllOwnersQueryHandler
        : IRequestHandler<GetAllOwnersQuery, PaginatedOwnersResult>
    {
        private readonly IOwnerRepository _repository;

        public GetAllOwnersQueryHandler(IOwnerRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedOwnersResult> Handle(
     GetAllOwnersQuery request, CancellationToken cancellationToken)
        {
            var (owners, totalRecords) = await _repository.GetAllPagesAsync(
                request.Page, request.PageSize, request.Search);

            var items = new List<OwnerDTO>();
            foreach (var o in owners)
            {
                var dto = new OwnerDTO
                {
                    Id = o.Id,
                    OwnerName = o.OwnerName,
                    Email = o.Email,
                    PhoneNumber = o.PhoneNumber,
                    IdCard = o.IdCard,
                    Gender = o.Gender
                };
                items.Add(dto);
            }

            return new PaginatedOwnersResult
            {
                Items = items,
                TotalRecords = totalRecords,
                TotalPages = totalRecords > 0
                    ? (int)Math.Ceiling((double)totalRecords / request.PageSize)
                    : 1
            };
        }
    }
}
