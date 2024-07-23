using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetApiCourse.Validations
{
    public class FirstUpperCaseAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var firstChar = value.ToString()[0].ToString();

            if (firstChar.Equals(firstChar.ToUpper()))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Name must start with capital letter");
        }
    }
}