using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetApiCourse.DTOs
{
    public class AuthorDTOwBooks : AuthorDTO
    {
        public List<BookDTO> Books { get; set; }
    }
}