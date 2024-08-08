using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetApiCourse.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}