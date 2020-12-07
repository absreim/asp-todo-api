using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Services;
using Xunit;

namespace TodoApiTests
{
    public class TodoItemsControllerTests
    {
        [Fact]
        public async Task GetTodoItem_Returns_NotFound_Given_Nonexistent_Id()
        {
            string id = "foo";
            var mockTodoService = new Mock<ITodoItemService>();
            mockTodoService.Setup(service => service.GetOne(id))
                .ReturnsAsync((TodoItem) null);
            var controller = new TodoItemsController(mockTodoService.Object);

            var result = await controller.GetTodoItem(id);
            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}