using System.Collections.Generic;
using System.Threading.Tasks;

using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoItemService
    {
        Task<List<TodoItem>> GetAll();
        Task<TodoItem> GetOne(string id);
        Task<TodoItem> Add(TodoItem todoItem);
        Task<TodoItem> Replace(TodoItem todoItem);
        Task<TodoItem> Delete(string id);
    }
}