namespace Data.Repository
{
    using Domain.Model;
    using Ether.Outcomes;

    public interface IDogShelterRepository
    {
        Task<IOutcome<List<Dog>>> GetAllDogs();

        Task<IOutcome> RegisterDog(Dog dog);
    }
}