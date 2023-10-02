namespace Application.Container
{
    using Application.Adapter;
    using Application.ExternalApiClient;
    using Application.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationContainer
    {
        public static IServiceCollection SetupApplication(this IServiceCollection services)
        {
            return services
                .AddSingleton<IExternalDogApiClient, ExternalDogApiClient>()
                .AddSingleton<IDogToDogDtoAdapter, DogToDogDtoAdapter>()
                .AddSingleton<IDogShelterService, DogShelterService>();
        }
    }
}