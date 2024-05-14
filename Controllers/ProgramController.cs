using Microsoft.AspNetCore.Mvc;
using Program_Form_Backend_API.Models;
using Microsoft.Azure.Cosmos;
using Program_Form_Backend_API.Services;

namespace Program_Form_Backend_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProgramsController : ControllerBase
    {
        private readonly DBService _dbService;

        public ProgramsController()
        {
            _dbService = DBService.Instance;
        }


        [HttpPost]
        public async Task<IActionResult> CreateProgram([FromBody] CompanyProgram program)
        {
            try
            {
                await _dbService.ProgramContainer.CreateItemAsync(program, new PartitionKey(program.ProgramId));

                return CreatedAtAction(nameof(GetProgramById), new { id = program.Id }, program);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgramById(string id)
        {
            try
            {
                var programResponse = await _dbService.ProgramContainer.ReadItemAsync<CompanyProgram>(id, new PartitionKey(id));
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