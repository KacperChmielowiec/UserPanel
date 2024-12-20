using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Models.Password;
using UserPanel.Interfaces;
namespace UserPanel.ViewComponents
{
    [Authorize(policy: "admin")]
    [ViewComponent(Name = "PasswordProtection")]
    public class PasswordProtectionViewComponent : ViewComponent
    {
        readonly IDataBaseProvider _databaseProvider;
        public PasswordProtectionViewComponent(IDataBaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public IViewComponentResult Invoke()
        {
            var rules = _databaseProvider.GetUserRepository().GetUserPasswordRules().ToDictionary(r => r.ConstraintType, r => r);

            PasswordProtectionForm form = new PasswordProtectionForm() {};


            if(rules.ContainsKey(PasswordConstraintType.ContainsUppercaseLetter))
            {
                form.IsUpperCase = rules[PasswordConstraintType.ContainsUppercaseLetter].IsEnabled;
            }

            if (rules.ContainsKey(PasswordConstraintType.ContainsSpecialCharacter))
            {
                form.IsSpecialChar = rules[PasswordConstraintType.ContainsSpecialCharacter].IsEnabled;
            }

            if (rules.ContainsKey(PasswordConstraintType.ContainsDigitNotAtStart))
            {
                form.IsDigit = rules[PasswordConstraintType.ContainsDigitNotAtStart].IsEnabled;
            }

            if (rules.ContainsKey(PasswordConstraintType.NoRepeatingCharacters))
            {
                form.IsNoRepeat = rules[PasswordConstraintType.NoRepeatingCharacters].IsEnabled;
            }

            if (rules.ContainsKey(PasswordConstraintType.LengthBetween))
            {
                form.IsLenRange = rules[PasswordConstraintType.LengthBetween].IsEnabled;
                form.MinValueLen = rules[PasswordConstraintType.LengthBetween].MinValue;
                form.MaxValueLen = rules[PasswordConstraintType.LengthBetween].MaxValue;
            }
            if(rules.ContainsKey(PasswordConstraintType.DaysToPasswordReset))
            {
                form.PassTimeValue = rules[PasswordConstraintType.DaysToPasswordReset].Value;
                form.IsPassTime = rules[PasswordConstraintType.DaysToPasswordReset].IsEnabled;
            }

            return View(form);
        }
    }
}
