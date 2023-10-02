namespace Application.Tests.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Application.Adapter;
    using Application.ExternalApiClient;
    using Application.Services;
    using Crosscutting.Outcomes;
    using Crosscutting.Services;
    using Data.Repository;
    using Domain.Model;
    using Domain.Model.ExternalDogApi;
    using Dto;
    using Ether.Outcomes;
    using FluentAssertions;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DogShelterServiceTests
    {
        private readonly Mock<ICalculationService> mockCalculationService = new Mock<ICalculationService>();
        private readonly Mock<IDogShelterRepository> mockDogShelterRepository = new Mock<IDogShelterRepository>();
        private readonly Mock<IDogToDogDtoAdapter> mockDtoAdapter = new Mock<IDogToDogDtoAdapter>();
        private readonly Mock<IExternalDogApiClient> mockExternalDogApiClient = new Mock<IExternalDogApiClient>();

        [Fact]
        public void DogShelterService_Create_WithNull_CalculationService_Failure()
        {
            Assert.Throws<ArgumentNullException>(() => new DogShelterService(mockExternalDogApiClient.Object, null, mockDogShelterRepository.Object, mockDtoAdapter.Object));
        }

        [Fact]
        public async Task DogShelterService_FindByBreed_Success()
        {
            //Arrange
            var expectedDogDto = new DogDto("dog", "breed", 2);

            mockDogShelterRepository
                .Setup(x => x.GetAllDogs())
                .ReturnsAsync(Outcomes.Success(new List<Dog>()));
            mockDtoAdapter
                .Setup(x => x.AdaptDogToDogDto(It.IsAny<List<Dog>>()))
                .Returns(new List<DogDto>() { expectedDogDto });

            var dogShelterService = new DogShelterService(mockExternalDogApiClient.Object,
                mockCalculationService.Object, mockDogShelterRepository.Object, mockDtoAdapter.Object);

            var breed = "canaan";
            var expectedResult = Outcomes.Success(new List<DogDto>() { expectedDogDto });

            //Act

            var result = await dogShelterService.FindByBreed(breed);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task DogShelterService_FindBySize_Success()
        {
            //Arrange
            var expectedDog = new Dog()
            {
                DogId = 1,
                AverageHeight = 1,
                Breed = "breed",
                Name = "Name",
                Temperament = "Temperament"
            };

            mockDogShelterRepository
                .Setup(x => x.GetAllDogs())
                .ReturnsAsync(Outcomes.Success(new List<Dog>() { expectedDog }));

            var dogShelterService = new DogShelterService(mockExternalDogApiClient.Object,
                mockCalculationService.Object, mockDogShelterRepository.Object, mockDtoAdapter.Object);

            var size = EnumSize.Small;
            var expectedResult = Outcomes.Success(new List<DogDto>()
            {
                new DogDto("Name", "breed", 1)
            });

            //Act

            var result = await dogShelterService.FindBySize(size);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task DogShelterService_FindByTemperament_Success()
        {
            //Arrange
            var expectedDog = new Dog()
            {
                DogId = 1,
                AverageHeight = 1,
                Breed = "breed",
                Name = "Name",
                Temperament = "Dangerous"
            };

            mockDogShelterRepository
                .Setup(x => x.GetAllDogs())
                .ReturnsAsync(Outcomes.Success(new List<Dog>() { expectedDog }));

            mockDtoAdapter
                .Setup(x => x.AdaptDogToDogDto(It.IsAny<List<Dog>>()))
                .Returns(new List<DogDto>() { new DogDto("Name", "breed", 1) });

            var dogShelterService = new DogShelterService(mockExternalDogApiClient.Object,
                mockCalculationService.Object, mockDogShelterRepository.Object, mockDtoAdapter.Object);

            var temperament = "Dangerous";
            var expectedResult = Outcomes.Success(new List<DogDto>()
            {
                new DogDto("Name", "breed", 1)
            });

            //Act

            var result = await dogShelterService.FindByTemperament(temperament);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task DogShelterService_RegisterANewDog_Success()
        {
            //Arrange
            mockExternalDogApiClient
                .Setup(x => x.GetDogBreed(It.IsAny<string>()))
                .ReturnsAsync(Outcomes.Success(new DogExternalApiModel()
                {
                    Height = new HeightExternalApi
                    {
                        Metric = "test"
                    }
                }));
            mockCalculationService
                .Setup(x => x.GetAverageHeight(It.IsAny<string>()))
                .Returns(0);
            mockDogShelterRepository
                .Setup(x => x.RegisterDog(It.IsAny<Dog>()))
                .ReturnsAsync(Outcomes.Success().WithMessage(OutcomeMessages.SuccessRegisteredMessage));

            var dogShelterService = new DogShelterService(mockExternalDogApiClient.Object,
                mockCalculationService.Object, mockDogShelterRepository.Object, mockDtoAdapter.Object);

            var breed = "canaan";
            var name = "dogtest";
            var expectedResult = Outcomes.Success().WithMessage(OutcomeMessages.SuccessRegisteredMessage);

            //Act

            var result = await dogShelterService.RegisterANewDog(name, breed);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}