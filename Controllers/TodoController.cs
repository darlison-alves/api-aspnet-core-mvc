using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;
            if(_context.TodoItems.Count() == 0) 
            {
                _context.TodoItems.Add(new TodoItem {Name = "Item1"});
                _context.SaveChanges();
            }
        }
        //GET: api/todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoitem = await _context.TodoItems.FindAsync(id);
            if(todoitem == null)
                return NotFound();
            
            return todoitem;
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTodoItem", new {id = todoItem.Id}, todoItem);
        }
    }
}