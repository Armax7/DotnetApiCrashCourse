using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetApiCourse.Entities;
using dotnetApiCourse.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetApiCourse.Controllers
{
    [ApiController]
    [Route("api/authors")]
    //[Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger<AuthorsController> logger;

        public AuthorsController(AppDbContext context, ILogger<AuthorsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        [HttpGet("list")]
        [ResponseCache(Duration = 5)]
        [ServiceFilter(typeof(ActionFilter))]
        public async Task<ActionResult<List<Author>>> Get([FromQuery] string name)
        {
            logger.LogWarning("Obtaining Authors");
            if (name == null)
            {
                return await context.Authors.Include(x => x.Books).ToListAsync();
            }
            return await context.Authors.Include(x => x.Books).Where(x => x.Name.Contains(name)).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Author>> GetById(int id)
        {
            var author = await context.Authors.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Author author)
        {
            var authorExists = await context.Authors.AnyAsync(x => x.Name == author.Name);

            if (authorExists)
            {
                return BadRequest("Author already exists");
            }

            context.Add(author);
            await context.SaveChangesAsync();
            return Created();
        }

        [HttpPut("{id:int}")] //api/authors/:id
        public async Task<ActionResult> Put(Author author, int id)
        {
            if (author.Id != id)
            {
                return BadRequest("Author's ID does not match any author on system");
            }
            context.Update(author);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var hasAuthor = await context.Authors.AnyAsync(x => x.Id == id);
            if (!hasAuthor)
            {
                return NotFound();
            }

            context.Remove(new Author() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}