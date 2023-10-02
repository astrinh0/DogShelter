namespace Application.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Adapter;
    using Application.ExternalApiClient;
    using Crosscutting.Outcomes;
    using Crosscutting.Services;
    using Data.Repository;
    using Dawn;
    using Domain.Model;
    using Dto;
    using Ether.Outcomes;

    internal class DogShelterService : IDogShelterService
    {
        private readonly ICalculationService calculationService;
        private readonly IDogShelterRepository dogShelterRepository;
        private readonly IDogToDogDtoAdapter dtoAdapter;
        private readonly IExternalDogApiClient externalDogApiClient;

        public DogShelterService(IExternalDogApiClient externalDogApiClient, ICalculationService calculationService,
            IDogShelterRepository dogShelterRepository, IDogToDogDtoAdapter dtoAdapter)
        {
            Guard.Argument(externalDogApiClient, nameof(externalDogApiClient)).NotNull();
            Guard.Argument(calculationService, nameof(calculationService)).NotNull();
            Guard.Argument(dogShelterRepository, nameof(dogShelterRepository)).NotNull();
            Guard.Argument(dtoAdapter, nameof(dtoAdapter)).NotNull();

            this.externalDogApiClient = externalDogApiClient;
            this.calculationService = calculationService;
            this.dogShelterRepository = dogShelterRepository;
            this.dtoAdapter = dtoAdapter;
        }

        public async Task<IOutcome<IEnumerable<DogDto>>> FindByBreed(string breed)
        {
            var allDogs = await this.dogShelterRepository.GetAllDogs();

            if (allDogs.Success)
            {
                var filteredDogs = this.dtoAdapter.AdaptDogToDogDto(allDogs.Value.Where(b => b.Breed == breed).ToList());
                if (!filteredDogs.Any())
                {
                    return Outcomes.Failure<IEnumerable<DogDto>>().WithMessage(OutcomeMessages.NoDogsFoundMessage);
                }
                return Outcomes.Success(filteredDogs);
            }

            return Outcomes.Failure<IEnumerable<DogDto>>().WithMessage(allDogs.Messages);
        }

        public async Task<IOutcome<IEnumerable<DogDto>>> FindBySize(EnumSize size)
        {
            var allDogs = await this.dogShelterRepository.GetAllDogs();

            if (allDogs.Success)
            {
                var filteredDogs = this.FilterDogs(allDogs.Value, size);

                if (!filteredDogs.Any())
                {
                    return Outcomes.Failure<IEnumerable<DogDto>>().WithMessage(OutcomeMessages.NoDogsFoundMessage);
                }
                return Outcomes.Success(filteredDogs);
            }

            return Outcomes.Failure<IEnumerable<DogDto>>().WithMessage(allDogs.Messages);
        }

        public async Task<IOutcome<IEnumerable<DogDto>>> FindByTemperament(string temperament)
        {
            var allDogsOutcome = await this.dogShelterRepository.GetAllDogs();

            if (!allDogsOutcome.Success || allDogsOutcome.Value == null)
            {
                return Outcomes.Failure<IEnumerable<DogDto>>().WithMessage(allDogsOutcome.Messages);
            }

            var filteredDogs = allDogsOutcome.Value
                .Where(dog =>
                    dog.Temperament != null &&
                    dog.Temperament
                        .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Any(adj => string.Equals(adj, temperament, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!filteredDogs.Any())
            {
                return Outcomes.Failure<IEnumerable<DogDto>>().WithMessage(OutcomeMessages.NoDogsFoundMessage);
            }

            return Outcomes.Success(this.dtoAdapter.AdaptDogToDogDto(filteredDogs));
        }

        public async Task<IOutcome> RegisterANewDog(string name, string breed)
        {
            var breedExternal = await this.externalDogApiClient.GetDogBreed(breed);

            if (breedExternal == null)
            {
                return Outcomes.Failure().WithMessage(OutcomeMessages.NoBreedFoundMessage);
            }

            if (breedExternal.Value.Height?.Metric == null)
            {
                return Outcomes.Failure().WithMessage(OutcomeMessages.NoHeightFoundMessage);
            }

            var averageHeight = this.calculationService.GetAverageHeight(breedExternal.Value.Height.Metric);

            var dog = new Dog
            {
                AverageHeight = averageHeight,
                Breed = breed,
                Name = name,
                Temperament = breedExternal.Value.Temperament
            };

            var result = await this.dogShelterRepository.RegisterDog(dog);

            return result;
        }

        private IEnumerable<DogDto> FilterDogs(List<Dog> dogs, EnumSize size)
        {
            return dogs
                .Where(dog => dog.Name != null && dog.Breed != null)
                .Where(dog =>
                    (size == EnumSize.Small && dog.AverageHeight < 35) ||
                    (size == EnumSize.Medium && dog.AverageHeight >= 35 && dog.AverageHeight < 55) ||
                    (size == EnumSize.Large && dog.AverageHeight >= 55))
                .Select(dog => new DogDto(dog.Name, dog.Breed, dog.AverageHeight))
                .ToList();
        }
    }
}