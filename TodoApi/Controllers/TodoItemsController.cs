using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        
        public TodoItemsController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _todoItemService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(string id)
        {
            var todoItem = await _todoItemService.GetOne(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            var createdItem = await _todoItemService.Add(todoItem);

            return CreatedAtAction(nameof(GetTodoItem), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(string id, TodoItem todoItem)
        {
            if (!id.Equals(todoItem.Id))
            {
                return BadRequest();
            }

            var updatedItem = await _todoItemService.Replace(todoItem);

            if (updatedItem == null)
            {
                return NotFound();
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(string id)
        {
            var todoItem = await _todoItemService.Delete(id);
            
            if (todoItem == null)
            {
                return NotFound();
            }
            
            return todoItem;
        }
    }
}