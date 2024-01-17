using System.ComponentModel.DataAnnotations;

namespace MyBGList.ValidationAttributes;

public class AllowedStringsAttribute: ValidationAttribute
{
    public HashSet<string> AllowedVaues { get; private set; }

    public AllowedStringsAttribute(string[] allowedValues) : base("Value must be one of the following: [{0}].")
    {
        ArgumentNullException.ThrowIfNull(allowedValues);
        AllowedVaues = new (allowedValues.Select(v => v.ToLower()));
    }


    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string? strValue = value as string;
        return string.IsNullOrEmpty(strValue) || !AllowedVaues.Contains(strValue.ToLower())
            ? new ValidationResult(FormatErrorMessage(string.Join(", ", AllowedVaues)))
            : ValidationResult.Success;
    }
}
