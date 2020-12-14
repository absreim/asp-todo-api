using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Services;
using Xunit;

namespace TodoApiUnitTests
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
            
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTodoItem_Returns_NonNull_Value_Given_Valid_Id()
        {
            string id = "foo";
            var mockTodoService = new Mock<ITodoItemService>();
            var todoItem = new TodoItem()
            {
                Id = "foo",
                Name = "Unit test item",
                IsComplete = false
            };
            mockTodoService.Setup(service => service.GetOne(id))
                .ReturnsAsync(todoItem);
            var controller = new TodoItemsController(mockTodoService.Object);

            var result = await controller.GetTodoItem(id);
            
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task PostTodoItem_Returns_CreatedAtActionResult_Matching_Input()
        {
            var todoItem = new TodoItem()
            {
                Id = "foo",
                Name = "Unit test item",
                IsComplete = false
            };
            var mockTodoService = new Mock<ITodoItemService>();
            mockTodoService.Setup(service => service.Add(todoItem))
                .ReturnsAsync(todoItem);
            var controller = new TodoItemsController(mockTodoService.Object);

            var result = await controller.PostTodoItem(todoItem);
            
            Assert.IsType<CreatedAtActionResult>(result.Result);
            var nestedResult = (CreatedAtActionResult)result.Result;
            Assert.True(TodoItemsEqual((TodoItem)nestedResult.Value, todoItem));
        }

        [Fact]
        public async Task PutTodoItem_Returns_BadRequest_If_Id_Does_Not_Match_Entity()
        {
            var todoItem = new TodoItem()
            {
                Id = "foo",
                Name = "Unit test item",
                IsComplete = false
            };
            var inputId = "bar";
            var mockTodoService = new Mock<ITodoItemService>();
            mockTodoService.Setup(service => service.Replace(todoItem))
                .ReturnsAsync(todoItem);
            var controller = new TodoItemsController(mockTodoService.Object);

            var result = await controller.PutTodoItem(inputId, todoItem);
            
            Assert.IsType<BadRequestResult>(result);
        }
        
        [Fact]
        public async Task PutTodoItem_Returns_NotFound_If_Entity_Does_Not_Exist()
        {
            var todoItem = new TodoItem()
            {
                Id = "foo",
                Name = "Unit test item",
                IsComplete = false
            };
            var inputId = "foo";
            var mockTodoService = new Mock<ITodoItemService>();
            mockTodoService.Setup(service => service.Replace(todoItem))
                .ReturnsAsync((TodoItem) null);
            var controller = new TodoItemsController(mockTodoService.Object);

            var result = await controller.PutTodoItem(inputId, todoItem);
            
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task PutTodoItem_Returns_NoContent_If_Update_Successful()
        {
            var todoItem = new TodoItem()
            {
                Id = "foo",
                Name = "Unit test item",
                IsComplete = false
            };
            var inputId = "foo";
            var mockTodoService = new Mock<ITodoItemService>();
            mockTodoService.Setup(service => service.Replace(todoItem))
                .ReturnsAsync(todoItem);
            var controller = new TodoItemsController(mockTodoService.Object);

            var result = await controller.PutTodoItem(inputId, todoItem);
            
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTodoItem_Returns_NotFound_If_Entity_Does_Not_Exist()
        {
            var id = "foo";
            var mockTodoService = new Mock<ITodoItemService>();
            mockTodoService.Setup(service => service.Delete(id))
                .ReturnsAsync((TodoItem)null);
            var controller = new TodoItemsController(mockTodoService.Object);

            var result = await controller.DeleteTodoItem(id);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteTodoItem_Returns_Deleted_Entity_If_Entity_Exists()
        {
            var todoItem = new TodoItem()
            {
                Id = "foo",
                Name = "Unit test item",
                IsComplete = false
            };
            var inputId = "foo";
            var mockTodoService = new Mock<ITodoItemService>();
            mockTodoService.Setup(service => service.Delete(inputId))
                .ReturnsAsync(todoItem);
            var controller = new TodoItemsController(mockTodoService.Object);

            var result = await controller.DeleteTodoItem(inputId);
            
            Assert.True(TodoItemsEqual(result.Value, todoItem));
        }

        private static bool TodoItemsEqual(TodoItem t1, TodoItem t2)
        {
            return t1.IsComplete == t2.IsComplete
                   && string.Equals(t1.Name, t2.Name)
                   && string.Equals(t1.Id, t2.Id);
        }
    }
}