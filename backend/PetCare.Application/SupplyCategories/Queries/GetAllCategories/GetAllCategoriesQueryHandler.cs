using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.SupplyCategories.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler
        : IRequestHandler<GetAllCategoriesQuery, List<SupplyCategoryDTO>>
    {
        private readonly ISupplyCategoryRepository _repository;

        public GetAllCategoriesQueryHandler(ISupplyCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SupplyCategoryDTO>> Handle(
            GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repository.GetAllAsync();

            var items = new List<SupplyCategoryDTO>();
            foreach (var c in categories)
            {
                items.Add(new SupplyCategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive
                });
            }

            return items;
        }
    }
}