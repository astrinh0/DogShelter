namespace Dto
{
    public class DogDto
    {
        public DogDto(string name, string breed, decimal averageHeight)
        {
            this.Name = name;
            this.Breed = breed;
            this.AverageHeight = averageHeight;
        }

        public decimal AverageHeight { get; set; }

        public string Breed { get; set; }

        public string Name { get; set; }
    }
}