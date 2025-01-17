using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dotnetApiCourse.Validations;

namespace dotnetApiCourse.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [FirstUpperCase]
        [StringLength(250, ErrorMessage = "Field {0} is to long, use less than {1} characters")]
        public string Title { get; set; }
        public DateTime? PublicationDate { get; set; }
        public List<Comment> Comments { get; set; }
        public List<AuthorBook> AuthorBook { get; set; }
    }
}