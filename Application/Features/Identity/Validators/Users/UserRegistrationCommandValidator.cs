using Application.Features.Identity.User.Commands;
using Common.Requests.Identity.Users;
using FluentValidation;

namespace Application.Features.Identity.Validators.Users
{
    public class UserRegistrationCommandValidator : AbstractValidator<UserRegistrationCommand>
    {
        public UserRegistrationCommandValidator()
        {

            RuleFor(request => request.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress()
                .MaximumLength(64);

            RuleFor(request => request.FirstName)
               .NotEmpty()
               .NotNull()
               .MaximumLength(32);

            RuleFor(request => request.LastName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(32);

            RuleFor(request => request.UserName)
                .NotEmpty()
                .NotNull().MaximumLength(32);

            RuleFor(request => request.Password)
                .NotEmpty();

            RuleFor(request => request.ComfirmPassword)
                .Must((req, confirmed) => req.Password == confirmed)
                .WithMessage("Passwords do not match.");
        }
    }
}
