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
    [Route("api/books/{bookId:int}/comments")]
    public class CommentsController : ControllerBase
    {
        private AppDbContext Context { get; }
        private IMapper Mapper { get; }
        public CommentsController(AppDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> GetByBook(int bookId)
        {
            var bookExists = await Context.Books.AnyAsync(book => book.Id == bookId);

            if (!bookExists)
            {
                return NotFound();
            }

            List<Comment> comments = await Context.Comments
            .Where(comment => comment.BookId == bookId).ToListAsync();

            return Mapper.Map<List<CommentDTO>>(comments);
        }

        [HttpGet("{id:int}", Name = "getCommentById")]
        public async Task<ActionResult<CommentDTO>> GetById(int id)
        {
            Comment comment = await Context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return Mapper.Map<CommentDTO>(comment);
        }

        [HttpPost]
        public async Task<ActionResult> PostComment(int bookId, CommentCreateDTO commentCreateDTO)
        {
            var bookExists = await Context.Books.AnyAsync(book => book.Id == bookId);

            if (!bookExists)
            {
                return NotFound();
            }

            Comment comment = Mapper.Map<Comment>(commentCreateDTO);
            comment.BookId = bookId;
            Context.Add(comment);
            await Context.SaveChangesAsync();

            CommentDTO commentDTO = Mapper.Map<CommentDTO>(comment);

            return CreatedAtRoute("getCommentById", new { id = comment.Id, bookId = bookId }, commentDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int bookId, int id, CommentUpdateDTO commentUpdateDTO)
        {
            bool bookExists = await Context.Books.AnyAsync(book => book.Id == bookId);
            if (!bookExists)
            {
                return NotFound("Book does not exist on system");
            }

            bool commentExists = await Context.Comments.AnyAsync(comment => comment.Id == id);
            if (!commentExists)
            {
                return NotFound("Comment does not exist on system");
            }

            Comment comment = Mapper.Map<Comment>(commentUpdateDTO);
            comment.Id = id;
            comment.BookId = bookId;

            Context.Update(comment);
            await Context.SaveChangesAsync();

            return NoContent();
        }
    }
}