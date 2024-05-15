using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Program_Form_Backend_API.Models;
using Program_Form_Backend_API.Services;

namespace Program_Form_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalInformationController : ControllerBase
    {
        private readonly DBService _dbService;

        public PersonalInformationController()
        {
            _dbService = DBService.Instance;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProgram([FromBody] PersonalInformation personalInformation)
        {
            try
            {
                await _dbService.PersonalInformationContainer.CreateItemAsync(personalInformation, new PartitionKey(personalInformation.ProgramId));

                return Ok(personalInformation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersonalInformation()
        {
            try
            {
                var personalInformationQuery = _dbService.PersonalInformationContainer.GetItemQueryIterator<PersonalInformation>();
                List<PersonalInformation> personalInformationList = new List<PersonalInformation>();

                while (personalInformationQuery.HasMoreResults)
                {
                    FeedResponse<PersonalInformation> response = await personalInformationQuery.ReadNextAsync();
                    personalInformationList.AddRange(response);
                }

                return Ok(personalInformationList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonalInformationById(string id)
        {
            try
            {
                var programResponse = await _dbService.ProgramContainer.ReadItemAsync<PersonalInformation>(id, new PartitionKey(id));
                return Ok(programResponse.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
