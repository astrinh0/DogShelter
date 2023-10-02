namespace Application.Services
{
    using Domain.Model;
    using Dto;
    using Ether.Outcomes;

    public interface IDogShelterService
    {
        Task<IOutcome<IEnumerable<DogDto>>> FindByBreed(string breed);

        Task<IOutcome<IEnumerable<DogDto>>> FindBySize(EnumSize size);

        Task<IOutcome<IEnumerable<DogDto>>> FindByTemperament(string temperament);

        Task<IOutcome> RegisterANewDog(string name, string breed);
    }
}