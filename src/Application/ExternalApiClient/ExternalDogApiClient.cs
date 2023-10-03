namespace Application.ExternalApiClient
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Crosscutting.Settings;
    using Dawn;
    using Domain.Model.ExternalDogApi;
    using Ether.Outcomes;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    internal class ExternalDogApiClient : IExternalDogApiClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string? url;

        public ExternalDogApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            Guard.Argument(httpClientFactory, nameof(httpClientFactory)).NotNull();
            Guard.Argument(configuration, nameof(configuration)).NotNull();

            this.httpClientFactory = httpClientFactory;
            this.url = configuration.GetSection(SettingsNames.ExternalDogsApi).Value;
        }

        public async Task<IOutcome<DogExternalApiModel>> GetDogBreed(string breed)
        {
            using (var request = httpClientFactory.CreateClient())
            {
                var response = await request.GetAsync(url + breed);

                if (response != null && response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var externalApiModels = JsonConvert.DeserializeObject<List<DogExternalApiModel>>(json);

                    if (externalApiModels != null && externalApiModels.Count > 0)
                    {
                        var externalApiModel = externalApiModels.FirstOrDefault();
                        if (externalApiModel != null)
                        {
                            return Outcomes.Success(externalApiModel);
                        }
                    }
                }
            }

            return Outcomes.Failure<DogExternalApiModel>();
        }
    }
}