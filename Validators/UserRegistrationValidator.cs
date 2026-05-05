using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using PharmaMicro.UserIdentityService.Models;
using PharmaMicro.UserIdentityService.Models.Enums;

namespace PharmaMicro.UserIdentityService.Validators
{
    public class UserRegistrationValidator : AbstractValidator<RegisterRequest>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithName("MailID")
                .WithMessage("Please Enter a valid Email address");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(8)
                .MaximumLength(12)
                .Must(IsPasswordValid)
                .WithMessage("Password should atleast have one Uppercase and an special character");

            RuleFor(x => x.FirstName.Trim())
                .Cascade(CascadeMode.Stop)
                .MinimumLength(4)
                .Must(IsNameValid)
                .WithMessage("FirstName should be with no special characters");

            RuleFor(x => x.LastName.Trim())
                .Cascade(CascadeMode.Stop)
                 .MaximumLength(8)
                 .Must(IsNameValid)
                 .WithMessage("FirstName should be with no special characters");

            RuleFor(x => x.Role.Trim().ToUpper())
                .Must(IsRoleValid)
                .WithMessage("Please enter a valid role");
        }

        private bool IsRoleValid(string arg)
        {
            return !string.IsNullOrWhiteSpace(arg)
                   && Enum.IsDefined(typeof(Role), arg);
        }

        private bool IsNameValid(string arg)
        {
            return !string.IsNullOrWhiteSpace(arg)
                   && arg.All(char.IsLetter);
        }

        private bool IsPasswordValid(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
                return false;

            bool hasUpper = arg.Any(char.IsUpper);
            bool hasSpecial = arg.Any(c => !char.IsLetterOrDigit(c));

            return hasUpper && hasSpecial;
        }

    }
}
