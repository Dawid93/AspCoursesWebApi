using CourseLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.ValidationAttributes
{
    public class CourseTitleMustBeDiffrent : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (CourseForCreationDto)validationContext.ObjectInstance;
            if(course.Title == course.Description)
            {
                return new ValidationResult(
                    "The Title and description should be diffrent", new[] { nameof(CourseForCreationDto) });
            }

            return ValidationResult.Success;
        }
    }
}
