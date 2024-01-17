using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyBGList.ValidationAttributes;

public class NameOfPropertyAttribute : ValidationAttribute
{
    private static Dictionary<Type, HashSet<string>> _entitiesAndProperties = new();

    private Type _entityType;

    public NameOfPropertyAttribute(Type entityType) : base("Value must be property name of entity {0}")
    {
        ArgumentNullException.ThrowIfNull(entityType);
        _entityType = entityType;

        if(_entitiesAndProperties.ContainsKey(_entityType)) return;

        HashSet<string> entityProperties = new(_entityType.GetProperties().Select(p => p.Name));
        _entitiesAndProperties.Add(entityType, entityProperties);
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string? propertyName = value as string;

        return string.IsNullOrEmpty(propertyName) || !_entitiesAndProperties[_entityType].Contains(propertyName)
            ? new ValidationResult(FormatErrorMessage(_entityType.Name))
            : ValidationResult.Success;
    }
}
