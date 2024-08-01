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
        public async Task<ActionResult<List<CommentDTO>>> Get(int bookId)
        {
            var bookExists = await Context.Books.AnyAsync(book => book.Id == bookId);

            if (!bookExists)
            {
                return NotFound();
            }
            
            List<Comment> comments = await Context.Comments
            .Where(comment => comment.Id == bookId).ToListAsync();

            return Mapper.Map<List<CommentDTO>>(comments);
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
            return Ok();
        }
    }
}