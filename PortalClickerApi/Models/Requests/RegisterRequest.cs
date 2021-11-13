using FluentValidation;

namespace PortalClickerApi.Models.Requests
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.UserName)
                .Length(1, 64);
            RuleFor(x => x.Email)
                .EmailAddress();
            RuleFor(x => x.Password)
                .Length(1, 128);
        }
    }
}
