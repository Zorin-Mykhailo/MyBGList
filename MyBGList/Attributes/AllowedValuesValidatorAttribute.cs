using System.ComponentModel.DataAnnotations;

namespace MyBGList.Attributes;

public class AllowedValuesValidatorAttribute: ValidationAttribute
{
    private HashSet<string> AllowedVaues { get; set; }

    public AllowedValuesValidatorAttribute(string[] allowedValues) : base("Value must be one of the following: {0}.")
    {
        ArgumentNullException.ThrowIfNull(allowedValues);
        AllowedVaues = new (allowedValues);
    }


    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string? strValue = value as string;
        return string.IsNullOrEmpty(strValue) || !AllowedVaues.Contains(strValue)
            ? new ValidationResult(FormatErrorMessage(string.Join(", ", AllowedVaues)))
            : ValidationResult.Success;
    }
}
