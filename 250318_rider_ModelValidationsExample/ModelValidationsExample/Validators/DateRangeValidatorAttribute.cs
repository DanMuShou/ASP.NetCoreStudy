using System.ComponentModel.DataAnnotations;

namespace ModelValidationsExample.Validators;

public class DateRangeValidatorAttribute : ValidationAttribute
{
    public string OtherPropertyName { get; set; } = string.Empty;

    public DateRangeValidatorAttribute()
    {
    }

    public DateRangeValidatorAttribute(string otherPropertyName)
    {
        OtherPropertyName = otherPropertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return null;

        var toDate = Convert.ToDateTime(value);
        var otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);
        if (otherProperty == null) return null;

        var fromDate = Convert.ToDateTime(otherProperty?.GetValue(validationContext.ObjectInstance));
        return fromDate > toDate
            ? new ValidationResult(ErrorMessage)
            : ValidationResult.Success;
    }
}