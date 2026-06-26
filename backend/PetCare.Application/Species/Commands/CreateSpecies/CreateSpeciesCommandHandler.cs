// Handler que guarda una nueva especie en la base de datos
using MediatR;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.Species.Commands.CreateSpecies
{
    public class CreateSpeciesCommandHandler : IRequestHandler<CreateSpeciesCommand, int?>
    {
        private readonly ISpeciesRepository _speciesRepository;

        public CreateSpeciesCommandHandler(ISpeciesRepository speciesRepository)
        {
            _speciesRepository = speciesRepository;
        }

        public async Task<int?> Handle(CreateSpeciesCommand request, CancellationToken cancellationToken)
        {
            // Creamos el objeto de dominio con el nombre recibido
            var specie = new PetCare.Domain.Models.Species
            {
                SpecieName = request.SpecieName
            };

            // Guardamos y devolvemos el Id generado
            var createdSpecie = await _speciesRepository.CreateSpecies(specie);
            return createdSpecie?.Id;
        }
    }
}