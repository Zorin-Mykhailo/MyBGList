using System.ComponentModel.DataAnnotations;

namespace MyBGList.ValidationAttributes;

public class NameOfPropertyAttribute : ValidationAttribute
{
    private static Dictionary<Type, HashSet<string>> _entitiesAndProperties = new();

    public Type EntityType { get; private set; }

    public HashSet<string> EntityProperties
    {
        get => _entitiesAndProperties[EntityType];
    }

    public NameOfPropertyAttribute(Type entityType) : base("Value must be property name of entity {0}")
    {
        ArgumentNullException.ThrowIfNull(entityType);
        EntityType = entityType;
        if (_entitiesAndProperties.ContainsKey(EntityType)) return;

        HashSet<string> entityProperties = new(EntityType.GetProperties().Select(p => p.Name));
        _entitiesAndProperties.Add(EntityType, entityProperties);
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string? propertyName = value as string;

        return string.IsNullOrEmpty(propertyName) || !EntityProperties.Contains(propertyName)
            ? new ValidationResult(FormatErrorMessage(EntityType.Name))
            : ValidationResult.Success;
    }
}