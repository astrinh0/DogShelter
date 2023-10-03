namespace Data.Repository
{
    using Dawn;
    using Domain.Model;
    using Ether.Outcomes;
    using Microsoft.Extensions.Configuration;

    internal class DogShelterRepository : IDogShelterRepository
    {
        private readonly IConfiguration configuration;

        public DogShelterRepository(IConfiguration configuration)
        {
            Guard.Argument(configuration, nameof(configuration)).NotNull();

            this.configuration = configuration;
        }

        public async Task<IOutcome<List<Dog>>> GetAllDogs()
        {
            try
            {
                using (var db = new DogsContext(this.configuration))
                {
                    db.Database.EnsureCreated();

                    return Outcomes.Success(db.Dogs.ToList());
                }
            }
            catch (Exception ex)
            {
                return Outcomes.Failure<List<Dog>>().WithMessage(ex.Message);
            }
        }

        public async Task<IOutcome> RegisterDog(Dog dog)
        {
            try
            {
                using (var db = new DogsContext(this.configuration))
                {
                    db.Database.EnsureCreated();

                    if (db.Dogs.Any(d => d.Name == dog.Name && d.Breed == dog.Breed))
                    {
                        return Outcomes.Failure().WithMessage("Already exists");
                    }

                    db.Dogs.Add(dog);
                    db.SaveChanges();
                }

                return Outcomes.Success();
            }
            catch (Exception ex)
            {
                return Outcomes.Failure().WithMessage(ex.Message);
            }
        }
    }
}