using DziekanatBackend.Database;
using FluentValidation;

namespace DziekanatBackend.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(DziekanatDbContext dbContext)
        {
            RuleFor(x => x.FirstName).NotEmpty();

            RuleFor(x => x.LastName).NotEmpty();

            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Password).MinimumLength(8);

            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

            RuleFor(x => x.Email).Custom((value, context) => 
            {
                if(dbContext.Lecturer.Any(e => e.Email == value) || dbContext.Student.Any(e => e.Email == value))
                {
                    context.AddFailure("Username", "Username is taken");
                }
            });
        }
    }
}
