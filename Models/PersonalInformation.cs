namespace Program_Form_Backend_API.Models
{
    public class PersonalInformation
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "nationality")]
        public string Nationality { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "resident")]
        public string Resident { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "nic")]
        public string NIC { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "date_of_birth")]
        public string DateOfBirth { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "programId")]
        public string ProgramId { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "answers")]
        public Dictionary<string, object> Answers { get; set; }

    }
}
