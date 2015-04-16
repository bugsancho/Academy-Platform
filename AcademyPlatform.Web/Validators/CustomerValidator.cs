using AcademyPlatform.Models.Courses;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcademyPlatform.Web.Validators
{
    public class CustomerValidator : AbstractValidator<Course>
    {
        public CustomerValidator()
        {
            RuleFor(course => course.Title).NotEmpty();
        }
    }
}