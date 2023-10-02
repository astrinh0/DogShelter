namespace Application.ExternalApiClient
{
    using System.Threading.Tasks;
    using Domain.Model.ExternalDogApi;
    using Ether.Outcomes;

    public interface IExternalDogApiClient
    {
        Task<IOutcome<DogExternalApiModel>> GetDogBreed(string breed);
    }
}