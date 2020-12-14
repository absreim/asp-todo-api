using System.Net.Http;
using Xunit;

namespace TodoApiFunctionalTests
{
    public class TodoItemsControllerTests : IClassFixture<TestFixture>
    {
        public HttpClient Client { get; }
        
        public TodoItemsControllerTests(TestFixture fixture)
        {
            Client = fixture.Client;
        }
        
        [Fact]
        public void Test1()
        {
        }
    }
}