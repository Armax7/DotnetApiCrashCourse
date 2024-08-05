using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetApiCourse.DTOs
{
    public class BookDTOwAuthors : BookDTO
    {
        public List<AuthorDTO> Authors { get; set; }
    }
}