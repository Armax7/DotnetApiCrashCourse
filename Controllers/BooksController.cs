using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnetApiCourse.DTOs;
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
        private readonly IMapper mapper;

        public BooksController(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDTOwAuthors>> Get(int id)
        {
            Book book = await context.Books
            .Include(book => book.AuthorBook)
            .ThenInclude(authorBook => authorBook.Author)
            .FirstOrDefaultAsync(x => x.Id == id);

            book.AuthorBook = [.. book.AuthorBook.OrderBy(authorBook => authorBook.Order)];

            return mapper.Map<BookDTOwAuthors>(book);
        }

        [HttpPost]
        public async Task<ActionResult> Post(BookCreateDTO bookCreateDTO)
        {
            if (bookCreateDTO.AuthorsIds == null)
            {
                return BadRequest("Cannot create book without authors.");
            }

            List<int> authorsIds = await context.Authors
            .Where(author => bookCreateDTO.AuthorsIds.Contains(author.Id))
            .Select(author => author.Id).ToListAsync();

            if (bookCreateDTO.AuthorsIds.Count != authorsIds.Count)
            {
                return BadRequest("One or more authors are not registered.");
            }

            Book book = mapper.Map<Book>(bookCreateDTO);

            if (book.AuthorBook != null)
            {
                for (int i = 0; i < book.AuthorBook.Count; i++)
                {
                    book.AuthorBook[i].Order = i;
                }
            }

            context.Add(book);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}