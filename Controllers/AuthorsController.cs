using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnetApiCourse.DTOs;
using dotnetApiCourse.Entities;
using dotnetApiCourse.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace dotnetApiCourse.Controllers
{
    [ApiController]
    [Route("api/authors")]
    //[Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AuthorsController(AppDbContext context, ILogger<AuthorsController> logger, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet]
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<List<AuthorDTOwBooks>>> Get([FromQuery] string name)
        {
            List<Author> authorList;
            if (name == null)
            {
                authorList = await context.Authors
                .Include(author => author.AuthorBook)
                .ThenInclude(authorBook => authorBook.Book)
                .ToListAsync();
            }
            else
            {
                authorList = await context.Authors
                .Where(author => author.Name.Contains(name))
                .Include(author => author.AuthorBook)
                .ThenInclude(authorBook => authorBook.Book)
                .ToListAsync();
            }

            return mapper.Map<List<AuthorDTOwBooks>>(authorList);
        }

        [HttpGet("{id:int}", Name = "getAuthorById")]
        public async Task<ActionResult<AuthorDTOwBooks>> GetById(int id)
        {
            var author = await context.Authors
            .Include(author => author.AuthorBook)
            .ThenInclude(authorBook => authorBook.Book)
            .FirstOrDefaultAsync(author => author.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return mapper.Map<AuthorDTOwBooks>(author);
        }

        [HttpPost]
        public async Task<ActionResult> Post(AuthorCreateDTO authorCreateDto)
        {
            var authorExists = await context.Authors.AnyAsync(author => author.Name == authorCreateDto.Name);

            if (authorExists)
            {
                return BadRequest("Author already exists");
            }

            Author author = mapper.Map<Author>(authorCreateDto);

            context.Add(author);
            await context.SaveChangesAsync();

            AuthorDTO authorDTO = mapper.Map<AuthorDTO>(author);

            return CreatedAtRoute("getAuthorById", new { id = author.Id }, authorDTO);
        }

        [HttpPut("{id:int}")] //api/authors/:id
        public async Task<ActionResult> Put(AuthorUpdateDTO authorUpdateDto, int id)
        {
            bool authorExists = await context.Authors.AnyAsync(author => author.Id == id);
            if (!authorExists)
            {
                return BadRequest("Author's ID does not match any author on system");
            }

            Author author = mapper.Map<Author>(authorUpdateDto);
            author.Id = id;

            context.Update(author);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var hasAuthor = await context.Authors.AnyAsync(author => author.Id == id);
            if (!hasAuthor)
            {
                return NotFound();
            }

            context.Remove(new Author() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}