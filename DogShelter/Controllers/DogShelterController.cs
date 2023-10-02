namespace DogShelter.WebApi.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using Application.Services;
    using Crosscutting.Outcomes;
    using Dawn;
    using Domain.Model;
    using Ether.Outcomes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class DogShelterController : Controller
    {
        private readonly IDogShelterService dogShelterService;

        public DogShelterController(IDogShelterService dogShelterService)
        {
            Guard.Argument(dogShelterService, nameof(dogShelterService)).NotNull();

            this.dogShelterService = dogShelterService;
        }

        [HttpGet]
        [Route("FindDogByBreed")]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindDogByBreed([Required] string breed)
        {
            var result = await this.dogShelterService.FindByBreed(breed);

            if (result.Success)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Messages);
        }

        [HttpGet]
        [Route("FindDogBySize")]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindDogBySize([Required] EnumSize size)
        {
            var result = await this.dogShelterService.FindBySize(size);

            if (result.Success)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Messages);
        }

        [HttpGet]
        [Route("FindDogByTemperament")]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindDogByTemperament([Required] string temperament)
        {
            var result = await this.dogShelterService.FindByTemperament(temperament);

            if (result.Success) { return Ok(result.Value); }

            return BadRequest(result.Messages);
        }

        [HttpPost]
        [Route("RegisterDog")]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterADog(
            [Required] string name,
            [Required] string breed)
        {
            var result = await this.dogShelterService.RegisterANewDog(name, breed);

            if (result.Success)
            {
                return Ok(Outcomes.Success().WithMessage(OutcomeMessages.SuccessRegisteredMessage));
            }

            return BadRequest(result.Messages);
        }
    }
}