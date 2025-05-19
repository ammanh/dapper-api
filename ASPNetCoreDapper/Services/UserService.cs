using ASPNetCoreDapper.Entities;
using FluentValidation;

namespace ASPNetCoreDapper.Services
{
    public class UserService
    {
        private readonly IValidator<User> _userValidator;

        public UserService(IValidator<User> userValidator)
        {
            _userValidator = userValidator;
        }

        public bool IsUserValid(User user)
        {
            var validationResult = _userValidator.Validate(user);
            return validationResult.IsValid;
        }

        public void ValidateUser(User user)
        {
            _userValidator.ValidateAndThrow(user);
        }
    }
} 