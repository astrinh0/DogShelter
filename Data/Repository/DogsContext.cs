namespace Data.Repository
{
    using Crosscutting.Settings;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class DogsContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DogsContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<Dog> Dogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.configuration.GetConnectionString(SettingsNames.DBConnectionString));
        }
    }
}