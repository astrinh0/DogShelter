namespace Application.Adapter
{
    using System.Collections.Generic;
    using Domain.Model;
    using Dto;

    public interface IDogToDogDtoAdapter
    {
        IEnumerable<DogDto> AdaptDogToDogDto(List<Dog> dogs);
    }
}