namespace Application.Adapter
{
    using System.Collections.Generic;
    using Domain.Model;
    using Dto;

    internal class DogToDogDtoAdapter : IDogToDogDtoAdapter
    {
        public IEnumerable<DogDto> AdaptDogToDogDto(List<Dog> dogs)
        {
            var listDogDto = new List<DogDto>();

            foreach (var dog in dogs)
            {
                var dogDto = new DogDto(dog.Name, dog.Breed, dog.AverageHeight);
                listDogDto.Add(dogDto);
            }

            return listDogDto;
        }
    }
}