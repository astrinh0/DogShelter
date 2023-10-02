namespace Data.Container
{
    using Data.Repository;
    using Microsoft.Extensions.DependencyInjection;

    public static class DataContainer
    {
        public static IServiceCollection SetupData(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDogShelterRepository, DogShelterRepository>();
        }
    }
}