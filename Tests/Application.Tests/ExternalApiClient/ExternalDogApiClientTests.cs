namespace Application.Tests.ExternalApiClient
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using Application.ExternalApiClient;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ExternalDogApiClientTests
    {
        private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
        private readonly string? mockUrl;

        [Fact]
        public void DogShelterController_Create_WithNull_DogShelterService_Failure()
        {
            Assert.Throws<ArgumentNullException>(() => new ExternalDogApiClient(null, null));
        }

        [Fact]
        public async Task DogShelterController_FindDogByBreed_Failure()
        {
            //Arrange
            mockDogShelterService.Setup(x => x.FindByBreed(It.IsAny<string>())).Returns(Task.FromResult<IOutcome<IEnumerable<DogDto>>>(Outcomes.Failure<IEnumerable<DogDto>>().WithMessage(OutcomeMessages.NoDogsFoundMessage)));
            var controller = new DogShelterController(mockDogShelterService.Object);
            var breed = "dog";
            var expectedResult = new BadRequestObjectResult(Outcomes.Failure<List<DogDto>>());
            expectedResult.Value = new List<string>() { OutcomeMessages.NoDogsFoundMessage };
            //Act

            var result = await controller.FindDogByBreed(breed);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task DogShelterController_FindDogByBreed_Success()
        {
            //Arrange
            mockDogShelterService.Setup(x => x.FindByBreed(It.IsAny<string>())).Returns(Task.FromResult<IOutcome<IEnumerable<DogDto>>>(Outcomes.Success<IEnumerable<DogDto>>(new List<DogDto>())));
            var controller = new DogShelterController(mockDogShelterService.Object);
            var breed = "canaan";
            var expectedResult = new OkObjectResult(Outcomes.Success<List<DogDto>>());
            expectedResult.Value = new List<DogDto>();

            //Act

            var result = await controller.FindDogByBreed(breed);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task DogShelterController_FindDogBySize_Failure()
        {
            //Arrange
            mockDogShelterService.Setup(x => x.FindBySize(It.IsAny<EnumSize>())).Returns(Task.FromResult<IOutcome<IEnumerable<DogDto>>>(Outcomes.Failure<IEnumerable<DogDto>>().WithMessage(OutcomeMessages.NoDogsFoundMessage)));
            var controller = new DogShelterController(mockDogShelterService.Object);
            var size = EnumSize.Medium;
            var expectedResult = new BadRequestObjectResult(Outcomes.Failure<List<DogDto>>());
            expectedResult.Value = new List<string>() { OutcomeMessages.NoDogsFoundMessage };
            //Act

            var result = await controller.FindDogBySize(size);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task DogShelterController_FindDogBySize_Success()
        {
            //Arrange
            mockDogShelterService.Setup(x => x.FindBySize(It.IsAny<EnumSize>())).Returns(Task.FromResult<IOutcome<IEnumerable<DogDto>>>(Outcomes.Success<IEnumerable<DogDto>>(new List<DogDto>())));
            var controller = new DogShelterController(mockDogShelterService.Object);
            var size = EnumSize.Medium;
            var expectedResult = new OkObjectResult(Outcomes.Success<List<DogDto>>());
            expectedResult.Value = new List<DogDto>();

            //Act

            var result = await controller.FindDogBySize(size);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task DogShelterController_FindDogByTemperament_Failure()
        {
            //Arrange
            mockDogShelterService.Setup(x => x.FindByTemperament(It.IsAny<string>())).Returns(Task.FromResult<IOutcome<IEnumerable<DogDto>>>(Outcomes.Failure<IEnumerable<DogDto>>().WithMessage(OutcomeMessages.NoDogsFoundMessage)));
            var controller = new DogShelterController(mockDogShelterService.Object);
            var temperament = "temperament";
            var expectedResult = new BadRequestObjectResult(Outcomes.Failure<List<DogDto>>());
            expectedResult.Value = new List<string>() { OutcomeMessages.NoDogsFoundMessage };
            //Act

            var result = await controller.FindDogByTemperament(temperament);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task DogShelterController_FindDogByTemperament_Success()
        {
            //Arrange
            mockDogShelterService.Setup(x => x.FindByTemperament(It.IsAny<string>())).Returns(Task.FromResult<IOutcome<IEnumerable<DogDto>>>(Outcomes.Success<IEnumerable<DogDto>>(new List<DogDto>())));
            var controller = new DogShelterController(mockDogShelterService.Object);
            var temperament = "Cautious";
            var expectedResult = new OkObjectResult(Outcomes.Success<List<DogDto>>());
            expectedResult.Value = new List<DogDto>();

            //Act

            var result = await controller.FindDogByTemperament(temperament);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task DogShelterController_RegisterADog_Failure()
        {
            //Arrange
            mockDogShelterService.Setup(x => x.RegisterANewDog(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<IOutcome>(Outcomes.Failure().WithMessage(OutcomeMessages.NoBreedFoundMessage)));
            var controller = new DogShelterController(mockDogShelterService.Object);
            var breed = "breedtest";
            var name = "dogtest";
            var expectedResult = new BadRequestObjectResult(Outcomes.Failure());
            expectedResult.Value = new List<string>() { OutcomeMessages.NoBreedFoundMessage };
            //Act

            var result = await controller.RegisterADog(name, breed);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task DogShelterController_RegisterADog_Success()
        {
            //Arrange
            mockDogShelterService.Setup(x => x.RegisterANewDog(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<IOutcome>(Outcomes.Success()));
            var controller = new DogShelterController(mockDogShelterService.Object);
            var breed = "canaan";
            var name = "dogtest";
            var expectedResult = new OkObjectResult(Outcomes.Success().WithMessage(OutcomeMessages.SuccessRegisteredMessage));

            //Act

            var result = await controller.RegisterADog(name, breed);

            //Assert

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}