using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetApiCourse.Entities;
using Microsoft.EntityFrameworkCore;

namespace dotnetApiCourse
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthorBook>().HasKey(authorBook => new { authorBook.AuthorId, authorBook.BookId });
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AuthorBook> AuthorBook { get; set; }
    }
}