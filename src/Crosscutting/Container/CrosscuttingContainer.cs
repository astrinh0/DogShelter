namespace Crosscutting.Container
{
    using Crosscutting.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class CrosscuttingContainer
    {
        public static IServiceCollection SetupCrosscutting(this IServiceCollection services)
        {
            return services
                .AddSingleton<ICalculationService, CalculationService>();
        }
    }
}