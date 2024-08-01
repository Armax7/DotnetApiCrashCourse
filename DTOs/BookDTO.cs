using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dotnetApiCourse.Validations;

namespace dotnetApiCourse.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}