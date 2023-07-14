namespace ItsyBitseList.IntegrationTests.SeededRepository
{
    public class TestBase : IClassFixture<TestApplicationFactory<Program>>
    {
        protected readonly TestApplicationFactory<Program> _factory;
        protected HttpClient _client;
        public TestBase(TestApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
    }
}
