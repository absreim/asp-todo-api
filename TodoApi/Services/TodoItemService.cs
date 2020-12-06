using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TodoApi.Exceptions;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoItemService
    {
        private readonly TodoContext _todoContext;

        public TodoItemService(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public async Task<List<TodoItem>> GetAll()
        {
            return await _todoContext.TodoItems.ToListAsync();
        }

        public async Task<TodoItem> GetOne(string id)
        {
            return await _todoContext.TodoItems.FindAsync(id);
        }

        public async Task<TodoItem> Add(TodoItem todoItem)
        {
            var entry = _todoContext.TodoItems.Add(todoItem);
            await _todoContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async void Replace(TodoItem todoItem)
        {
            var id = todoItem.Id;
            _todoContext.Entry(todoItem).State = EntityState.Modified;
            try
            {
                await _todoContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    throw new TodoItemNotFoundException();
                }

                throw;
            }
        }

        public async Task<TodoItem> Delete(string id)
        {
            var todoItem = await _todoContext.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return null;
            }

            _todoContext.TodoItems.Remove(todoItem);
            await _todoContext.SaveChangesAsync();

            return todoItem;
        }
        
        private bool TodoItemExists(string id)
        {
            return _todoContext.TodoItems.Any(e => e.Id.Equals(id));
        }
    }
}