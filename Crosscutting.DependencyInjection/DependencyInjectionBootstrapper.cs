namespace Crosscutting.DependencyInjection
{
    using Application.Container;
    using Crosscutting.Container;
    using Data.Container;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjectionBootstrapper
    {
        public static IServiceCollection RegisterServices(IServiceCollection services)
        {
            services
                .SetupApplication()
                .SetupData()
                .SetupCrosscutting();

            return services;
        }
    }
}