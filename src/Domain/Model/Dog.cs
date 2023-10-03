namespace Domain.Model
{
    using System.ComponentModel.DataAnnotations;

    public class Dog
    {
        [Required]
        public decimal AverageHeight { get; set; }

        [Required]
        public string? Breed { get; set; }

        [Key]
        public int DogId { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Temperament { get; set; }
    }
}