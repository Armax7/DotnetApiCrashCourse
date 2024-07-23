using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dotnetApiCourse.Validations;

namespace dotnetApiCourse.Entities
{
    public class Author : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        [FirstUpperCase]
        //[StringLength(50, ErrorMessage = "Field {0} is to long, use less than {1} characters")]
        public string Name { get; set; }
        public List<Book> Books { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name) && Name.Length > 50)
            {
                yield return new ValidationResult("Name is to long, use less than 50 characters");
            }
        }
    }
}