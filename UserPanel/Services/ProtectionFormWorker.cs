using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.OpenApi.Any;
using Org.BouncyCastle.Asn1.BC;
using UserPanel.Interfaces;
using UserPanel.Models.Password;
namespace UserPanel.Services
{
    public static class ProtectionFormWorker
    {
      
        public static void HandleProtectionForm(PasswordProtectionForm form, IDataBaseProvider provider)
        {
            if (form == null) return;

            var rules = provider.GetUserRepository().GetUserPasswordRules().ToDictionary((rule) => rule.ConstraintType, rule => rule);

            void UpdateCheckBox(PasswordConstraintType type, bool value)
            {
                if (rules.ContainsKey(type))
                {
                    var rule = rules[type];
                    rule.IsEnabled = value;
                    provider.GetUserRepository().UpdatePasswordRule(PasswordConstraintType.ContainsUppercaseLetter, rule);
                }
            }

            void UpdateMinMaxRange(PasswordConstraintType type, bool isEnable, int min, int max)
            {
                if (rules.ContainsKey(type) && isEnable)
                {
                    var rule = rules[type];
                    rule.MinValue = min;
                    rule.MaxValue = max;
                    rule.IsEnabled = isEnable;
                    provider.GetUserRepository().UpdatePasswordRule(type, rule);
                }
                else if(rules.ContainsKey(type) && !isEnable)
                {
                    var rule = rules[type];
                    rule.IsEnabled = isEnable;
                    provider.GetUserRepository().UpdatePasswordRule(type, rule);
                }

            }

            void UpdateValueText(PasswordConstraintType type, bool isEnable, int value)
            {
                if (rules.ContainsKey(type) && isEnable)
                {
                    var rule = rules[type];
                    rule.Value = value;
                    rule.IsEnabled = isEnable;
                    provider.GetUserRepository().UpdatePasswordRule(type, rule);
                }
                else if (rules.ContainsKey(type) && !isEnable)
                {
                    var rule = rules[type];
                    rule.IsEnabled = isEnable;
                    provider.GetUserRepository().UpdatePasswordRule(type, rule);
                }

            }

            UpdateCheckBox(PasswordConstraintType.ContainsUppercaseLetter, form.IsUpperCase);
            UpdateCheckBox(PasswordConstraintType.ContainsDigitNotAtStart, form.IsDigit);
            UpdateCheckBox(PasswordConstraintType.NoRepeatingCharacters, form.IsNoRepeat);
            UpdateCheckBox(PasswordConstraintType.ContainsSpecialCharacter,form.IsSpecialChar);
            UpdateMinMaxRange(PasswordConstraintType.LengthBetween,form.IsLenRange, form.MinValueLen, form.MaxValueLen);
            UpdateValueText(PasswordConstraintType.DaysToPasswordReset,form.IsPassTime,form.PassTimeValue);
        }

    }
}
