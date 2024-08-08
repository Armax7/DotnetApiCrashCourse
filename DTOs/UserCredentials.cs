using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetApiCourse.DTOs
{
    public class UserCredentials
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}