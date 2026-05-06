using FluentValidation;
using FluentValidation.Validators;
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
            RuleFor(x => x.Email).
                Matches(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
                .WithName("MailID")
                .WithMessage("Please Enter a valid Email address");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(8)
                .MaximumLength(12)
                .Must(IsPasswordValid)
                .WithMessage("Password should atleast have one Uppercase and an special character");

            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(4)
                .Must(IsFirstNameValid)
                .WithMessage("FirstName should be with no special characters");

            RuleFor(x => x.LastName)
                 .Cascade(CascadeMode.Stop)
                 .MaximumLength(8)
                 .Must(IsLastNameValid)
                 .WithMessage("LastName should not have Special characters or numbers");

            RuleFor(x => x.Role)
                .Must(IsRoleValid)
                .WithMessage("Please enter a valid role");
        }

        private bool IsRoleValid(string arg)
        {
            var newVal = arg.Trim().ToUpper();
            return !string.IsNullOrWhiteSpace(newVal)
                   && Enum.IsDefined(typeof(Role), newVal);
        }

        private bool IsFirstNameValid(string arg)
        {
            var trimmedval = arg.Trim();

            return !string.IsNullOrWhiteSpace(trimmedval)
                   && trimmedval.All(char.IsLetter);
        }
        private bool IsLastNameValid(string arg)
        {
            var trimmedval = arg.Trim();

            if (!trimmedval.IsNullOrEmpty())
            {
                return trimmedval.All(char.IsLetter);
            }
            return true;
        }

        private bool IsPasswordValid(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
                return false;

            bool hasUpper = arg.Any(char.IsUpper);
            bool hasSpecial = arg.Any(c => !char.IsLetterOrDigit(c));
            bool hasDigit = arg.Any(c => char.IsDigit(c));

            return hasUpper && hasSpecial && hasDigit;
        }

    }
}
