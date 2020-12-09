using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoItemService: ITodoItemService
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

        public async Task<TodoItem> Replace(TodoItem todoItem)
        {
            _todoContext.Entry(todoItem).State = EntityState.Modified;
            try
            {
                await _todoContext.SaveChangesAsync();
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }

            return todoItem;
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
    }
}