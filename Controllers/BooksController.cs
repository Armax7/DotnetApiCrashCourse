using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnetApiCourse.DTOs;
using dotnetApiCourse.Entities;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id:int}", Name = "getBookById")]
        public async Task<ActionResult<BookDTOwAuthors>> Get(int id)
        {
            Book book = await context.Books
            .Include(book => book.AuthorBook)
            .ThenInclude(authorBook => authorBook.Author)
            .FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return NotFound("Book not found");
            }

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

            AssignOrderToAuthors(book);

            context.Add(book);
            await context.SaveChangesAsync();

            BookDTO bookDTO = mapper.Map<BookDTO>(book);

            return CreatedAtRoute("getBookById", new { id = book.Id }, bookDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(BookUpdateDTO bookUpdateDTO, int id)
        {
            //Entity Framework Core remembers everything that has been assigned to "book"
            Book book = await context.Books
            .Include(book => book.AuthorBook)
            .FirstOrDefaultAsync(book => book.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            //Becaus EF Core remembers "book" this updates with presisting changes
            book = mapper.Map(bookUpdateDTO, book);
            AssignOrderToAuthors(book);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<BookPatchDTO> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
                return BadRequest();
            }

            Book book = await context.Books.FirstOrDefaultAsync(book => book.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            BookPatchDTO bookPatchDTO = mapper.Map<BookPatchDTO>(book);

            jsonPatchDocument.ApplyTo(bookPatchDTO, ModelState);

            bool isValidModel = TryValidateModel(bookPatchDTO);
            if (!isValidModel)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(bookPatchDTO, book);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var hasBook = await context.Books.AnyAsync(book => book.Id == id);
            if (!hasBook)
            {
                return NotFound();
            }

            context.Remove(new Book() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

        private static void AssignOrderToAuthors(Book book)
        {
            if (book.AuthorBook != null)
            {
                for (int i = 0; i < book.AuthorBook.Count; i++)
                {
                    book.AuthorBook[i].Order = i;
                }
            }

        }
    }
}