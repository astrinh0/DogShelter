namespace Domain.Model.ExternalDogApi
{
    public class DogExternalApiModel
    {
        public string? Bred_for { get; set; }
        public string? Breed_group { get; set; }
        public HeightExternalApi? Height { get; set; }
        public int Id { get; set; }

        public string? Life_Span { get; set; }
        public string? Name { get; set; }
        public int? Reference_image_id { get; set; }

        public string? Temperament { get; set; }

        public WeightExternalApi? Weight { get; set; }
    }
}