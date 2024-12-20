using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UserPanel.Models.Dashboard;
using UserPanel.Models.Password;
using UserPanel.Models.User;

namespace UserPanel.Providers
{
    public static class PasswordValidationHelper
    {
        public static bool ValidatePasswordByRules(AppDbContext context, string password)
        {
            // Pobierz reguły haseł przypisane do użytkownika
            var passwordRules = context.PasswordRules;

            // Sprawdź każdą regułę
            foreach (var rule in passwordRules)
            {
                if (!ValidatePasswordByRule(password, rule))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidatePasswordByRule(string password, PasswordRules rule)
        {
            if (!rule.IsEnabled) return true;
            if (!string.IsNullOrEmpty(rule?.Regex))
            {
                return System.Text.RegularExpressions.Regex.IsMatch(password, rule.Regex);
            }
            if (rule.ConstraintType == PasswordConstraintType.LengthBetween)
            {
                return password.Length >= rule.MinValue && password.Length <= rule.MaxValue;
            }
            if(rule.ConstraintType == PasswordConstraintType.NoRepeatingCharacters)
            {
                return password.Length == password.Distinct().Count();
            }
            return true;
        }
    }

    public class ResetUserPasswordValidatorEmail: AbstractValidator<ResetPasswordUserModel>
    {
        private readonly AppDbContext _context;

        public ResetUserPasswordValidatorEmail(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.password)
                .Equal(x => x.cpassword).WithMessage("Passwords must match.");

            RuleFor(x => x.password)
                .Must((userResetPassword, newPassword) => PasswordValidationHelper.ValidatePasswordByRules(_context, newPassword))
                .WithMessage("Password does not meet all criteria enabled by Admin.");
        }
    }

    public class ResetUserPasswordValidatorDashboard : AbstractValidator<ResetUserPassword>
    {
        private readonly AppDbContext _context;

        public ResetUserPasswordValidatorDashboard(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Password is required.")
            .Equal((x) => x.NewPassword).WithMessage("Confirm Password must match to new Password");

            RuleFor(x => x.NewPassword)
                .Must((userResetPassword, newPassword) => PasswordValidationHelper.ValidatePasswordByRules(_context, newPassword))
                .WithMessage("Password does not meet all criteria enabled by Admin.");
        }
    }
}
