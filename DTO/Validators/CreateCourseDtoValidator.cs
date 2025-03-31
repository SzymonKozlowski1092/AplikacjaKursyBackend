using DziekanatBackend.Database;
using FluentValidation;

namespace DziekanatBackend.Models.Validators
{
    public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDtoValidator(DziekanatDbContext dbContext)
        {
            RuleFor(c => c.Name).Custom((value, context) =>
            {
                if (dbContext.Course.Any(e => e.Name == value))
                {
                    context.AddFailure("Name", "Istnieje już kurs o tej nazwie");
                }
            });
        }
    }
}
