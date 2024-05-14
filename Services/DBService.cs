using Microsoft.Azure.Cosmos;

namespace Program_Form_Backend_API.Services
{
    public class DBService
    {
        private static readonly DBService _instance = new DBService();
        public CosmosClient _cosmosClient;
        private Container _container;

        private const string EndpointUrl = "https://localhost:8081";
        private const string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private const string DatabaseId = "ToDoTaskDB";
        private const string ProgramContainerId = "Program";
        private const string PersonalInformationId = "PersonalInformation";
        private const string QuestionId = "Question";

        public static DBService Instance => _instance;

        public Container ProgramContainer
        {
            get
            {
                if (_container == null)
                {
                    InitializeContainer(ProgramContainerId, "/programId").Wait();
                }
                return _container;
            }
        }

        public Container PersonalInformationContainer
        {
            get
            {
                if (_container == null)
                {
                    InitializeContainer(PersonalInformationId, "/programId").Wait();
                }
                return _container;
            }
        }

        public Container QuestionContainer
        {
            get
            {
                if (_container == null)
                {
                    InitializeContainer(QuestionId, "/questionType").Wait();
                }
                return _container;
            }
        }

        private async Task InitializeContainer(string ContainerId, string PartitionKey)
        {
            _cosmosClient = new CosmosClient(EndpointUrl, PrimaryKey);
            Database database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
            _container = await database.CreateContainerIfNotExistsAsync(ContainerId, PartitionKey);
        }

        private DBService()
        {
        }
    }
}
