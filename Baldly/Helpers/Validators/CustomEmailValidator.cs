using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Baldly.Helpers.Validators;

public class CustomEmailValidator : ValidationAttribute
{
    private const string EmailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return ValidationResult.Success;

        var emailAddress = value.ToString();
        if (emailAddress != null && Regex.IsMatch(emailAddress, EmailPattern)) return ValidationResult.Success;

        return new ValidationResult(ErrorMessage);
    }
}