using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetApiCourse.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetApiCourse.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext context;

        public BooksController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            return await context.Books.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Book book)
        {
            var hasAuthor = await context.Authors.AnyAsync(x => x.Id == book.AuthorId);

            if (!hasAuthor)
            {
                return BadRequest($"Author ID: {book.AuthorId} doesn't exist on system");
            }
            context.Add(book);
            await context.SaveChangesAsync();
            return Created();
        }
    }
}