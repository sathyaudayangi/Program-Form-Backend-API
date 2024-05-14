using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Program_Form_Backend_API.Models
{
    public enum QuestionType
    {
        [EnumMember(Value = "Paragraph")]
        Paragraph,

        [EnumMember(Value = "YesNo")]
        YesNo,

        [EnumMember(Value = "Number")]
        Number,

        [EnumMember(Value = "Dropdown")]
        Dropdown,

        [EnumMember(Value = "MultipleChoice")]
        MultipleChoice,

        [EnumMember(Value = "Date")]
        Date
    }

    public class Question
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "type")]
        public QuestionType Type { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "program_id")]
        public string ProgramId { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "questionType")]
        public String QuestionType { get { return Type.ToString(); } }
    }

    public class DropdownQuestion : Question
    {
        public DropdownQuestion()
        {
            Type = Models.QuestionType.Dropdown;
        }
        [JsonProperty(PropertyName = "choices")]
        public List<string> Choices { get; set; }
    }

    public class MultipleChoiceQuestion : Question
    {
        public MultipleChoiceQuestion()
        {
            Type = Models.QuestionType.MultipleChoice;
        }

        [JsonProperty(PropertyName = "choices")]
        public List<string> Choices { get; set; }

        [JsonProperty(PropertyName = "max_choices")]
        public int MaxChoices { get; set; }
    }

    public class UpdateQuestion
    {
        public string description { get; set; }
        public QuestionType type { get; set; }
        public int maxChoise { get; set; }
        public List<string> choices { get; set; }

    }
}
