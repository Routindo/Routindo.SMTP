using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Routindo.Plugins.SMTP.UI.ValidationRules
{
    public class EmailValidationRule: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = value?.ToString();
            if (string.IsNullOrWhiteSpace(email))
            {
                return new ValidationResult(false, "Email cannot be empty");
            }

            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(email) ? ValidationResult.ValidResult : new ValidationResult(false, "Email must be in format 'username@domain.extension'");
        }
    }
}
