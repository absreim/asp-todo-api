using System.Net.Http;
using System.Text;
using TodoApi.Models;
using Xunit;
using System.Text.Json;
using System.Threading.Tasks;

namespace TodoApiFunctionalTests
{
    public class TodoItemsControllerTests : IClassFixture<TestFixture>
    {
        private HttpClient Client { get; }
        
        public TodoItemsControllerTests(TestFixture fixture)
        {
            Client = fixture.Client;
        }
        
        [Fact]
        public async Task Post_Is_Successful()
        {
            var todoItem = new TodoItem()
            {
                Id = "foo",
                IsComplete = false,
                Name = "Test post"
            };
            var jsonString = JsonSerializer.Serialize(todoItem);
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("api/TodoItems", stringContent);
            response.EnsureSuccessStatusCode();
        }
    }
}