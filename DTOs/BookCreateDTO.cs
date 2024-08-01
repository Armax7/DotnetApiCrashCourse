using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dotnetApiCourse.Validations;

namespace dotnetApiCourse.DTOs
{
    public class BookCreateDTO
    {
        [FirstUpperCase]
        [StringLength(250, ErrorMessage = "Field {0} is to long, use less than {1} characters")]
        public string Title { get; set; }
        public List<int> AuthorsIds { get; set; }
    }
}