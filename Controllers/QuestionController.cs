using Microsoft.AspNetCore.Mvc;
using ToDoTask.Models;
using Microsoft.Azure.Cosmos;
using ToDoTask.Services;
using Microsoft.Azure.Cosmos.Linq;
using System.ComponentModel;
using System.Net;
using Program_Form_Backend_API.Services;

namespace ToDoTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly DBService _dbService;

        public QuestionController()
        {
            _dbService = DBService.Instance;
        }

        //Store Paragraph, YesNo, Date, Number questions
        [HttpPost("baseQuestion")]
        public async Task<IActionResult> CreateQuestion([FromBody] Question question)
        {
            try
            {
                await _dbService.QuestionContainer.CreateItemAsync(question, new PartitionKey(question.QuestionType.ToString()));
                return Ok(question);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Store Multiple Choise questions
        [HttpPost("multipleChoise")]
        public async Task<IActionResult> CreateQuestion([FromBody] MultipleChoiceQuestion multipleChoiceQuestion)
        {
            try
            {
                multipleChoiceQuestion.Type = QuestionType.MultipleChoice;
                await _dbService.QuestionContainer.CreateItemAsync(multipleChoiceQuestion, new PartitionKey(multipleChoiceQuestion.QuestionType.ToString()));
                return Ok(multipleChoiceQuestion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Store DropDown questions
        [HttpPost("dropDown")]
        public async Task<IActionResult> CreateQuestion([FromBody] DropdownQuestion dropDownQuestion)
        {
            try
            {
                dropDownQuestion.Type = QuestionType.Dropdown;
                await _dbService.QuestionContainer.CreateItemAsync(dropDownQuestion, new PartitionKey(dropDownQuestion.QuestionType.ToString()));
                return Ok(dropDownQuestion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //Get all questions
        [HttpGet]
        public async Task<IActionResult> getAllQuestions()
        {
            try
            {
                var questionQuery = _dbService.QuestionContainer.GetItemQueryIterator<Question>();
                List<Question> questionList = new List<Question>();

                while (questionQuery.HasMoreResults)
                {
                    FeedResponse<Question> response = await questionQuery.ReadNextAsync();
                    questionList.AddRange(response);
                }

                return Ok(questionList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Get questions based on the question type.
        [HttpGet("{type}")]
        public async Task<IActionResult> GetQuestionsByType(QuestionType type)
        {
            var queryResult = _dbService.QuestionContainer.GetItemLinqQueryable<Question>()
                .Where(q => q.Type == type)
                .ToFeedIterator();

            var questions = new List<Question>();

            while (queryResult.HasMoreResults)
            {
                FeedResponse<Question> currentResult = await queryResult.ReadNextAsync();
                questions.AddRange(currentResult);
            }

            return Ok(questions);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(string id, [FromBody] UpdateQuestion question)
        {
            try
            {
                // Retrieve the item/document from the container based on the provided ID
                ItemResponse<Question> response = await _dbService.QuestionContainer.ReadItemAsync<Question>(id, new PartitionKey(question.previousType.ToString()));

                // Check if the question type is MultipleChoice
                if (question.previousType == QuestionType.MultipleChoice)
                {
                    MultipleChoiceQuestion multipleChoiceQuestion = new MultipleChoiceQuestion();
                    multipleChoiceQuestion.Id = id;
                    multipleChoiceQuestion.ProgramId = response.Resource.ProgramId;
                    multipleChoiceQuestion.Description = question.description;
                    multipleChoiceQuestion.Choices = question.choices;
                    multipleChoiceQuestion.MaxChoices = question.maxChoise;

                    await _dbService.QuestionContainer.ReplaceItemAsync(multipleChoiceQuestion, response.Resource.Id, new PartitionKey(multipleChoiceQuestion.QuestionType));

                }
                else if (question.previousType == QuestionType.Dropdown)
                {
                    DropdownQuestion dropdownQuestion = new DropdownQuestion();
                    dropdownQuestion.Id = id;
                    dropdownQuestion.ProgramId = response.Resource.ProgramId;
                    dropdownQuestion.Description = question.description;
                    dropdownQuestion.Choices = question.choices;

                    await _dbService.QuestionContainer.ReplaceItemAsync(dropdownQuestion, response.Resource.Id, new PartitionKey(dropdownQuestion.QuestionType));
                }
                else
                {
                    Question commonQuestion = new Question();
                    commonQuestion.Id = id;
                    commonQuestion.ProgramId = response.Resource.ProgramId;
                    commonQuestion.Description = question.description;
                    await _dbService.QuestionContainer.ReplaceItemAsync(commonQuestion, response.Resource.Id, new PartitionKey(commonQuestion.QuestionType));
                }
                return Ok("Question updated successfully");
            }
            catch (CosmosException ex)
            {
                // Handle exceptions
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(string id, string type)
        {
            try
            {
                // Read the item to check if it exists
                ItemResponse<Question> response = await _dbService.QuestionContainer.ReadItemAsync<Question>(id, new PartitionKey(type));

                // If the item exists, delete it
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await _dbService.QuestionContainer.DeleteItemAsync<Question>(id, new PartitionKey(type));
                    return Ok("Question deleted successfully.");
                }
                else
                {
                    return NotFound("Question not found.");
                }
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Handle case when the item is not found
                return NotFound("Question not found.");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
