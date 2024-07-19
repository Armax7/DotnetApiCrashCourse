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

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}