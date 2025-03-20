using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ModelValidationsExample.Validators;

namespace ModelValidationsExample.Model;

public class Person : IValidatableObject
{
    [Display(Name = "Person Name")]
    [Required(ErrorMessage = "MyError - {0} is required.")]
    [StringLength(10, MinimumLength = 3, ErrorMessage = "MyError - {0} must between {2} and {1} characters long.")]
    [RegularExpression("^[A-Za-z -_.]+$", ErrorMessage = "MyError - {0} should contain only alphabets, space and dot.")]
    public string? PersonName { get; set; }

    [Required(ErrorMessage = "MyError - {0} is required.")]
    public string? Password { get; set; }

    [Display(Name = "Re-enter Password")]
    [Required(ErrorMessage = "MyError - {0} Password is required.")]
    [Compare("Password", ErrorMessage = "MyError - {0} and {1} must match.")]
    public string? ConfirmPassword { get; set; }

    [Required(ErrorMessage = "MyError - {0} is required.")]
    [EmailAddress(ErrorMessage = "MyError - {0} is not a valid email address.")]
    public string? Email { get; set; }

    [ValidateNever]
    [Phone(ErrorMessage = "MyError - {0} is not a valid phone number.")]
    public string? Phone { get; set; }

    [Range(0, 999.99, ErrorMessage = "MyError - Price must be between {1} and {2}.")]
    public double? Price { get; set; }

    // [MinimumYearValidator(2007, ErrorMessage = "MyError - DateOfBrith must be greater than jan 01, {0}.")]
    [MinimumYearValidator(2007)] public DateTime? DateOfBrith { get; set; }

    public DateTime? FromDate { get; set; }

    [DateRangeValidator("FromDate", ErrorMessage = "From Date should be older than or equal to 'To date'")]
    public DateTime? ToDate { get; set; }

    public int? Age { get; set; }

    public List<string?> Tags { get; set; } = [];

    public override string ToString()
    {
        return $"Person Name: {PersonName}\n" +
               $"Password: {Password}\n" +
               $"ConfirmPassword: {ConfirmPassword}\n" +
               $"Email: {Email}\n" +
               $"Phone: {Phone}\n" +
               $"Price: {Price:F2}\n" +
               $"DateOfBrith: {DateOfBrith:d}\n";
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!DateOfBrith.HasValue && !Age.HasValue)
        {
            //yield 允许返回多个值
            yield return new ValidationResult(
                "Either of Date of Birth or Age must be supply", [nameof(Age)]);
        }
    }
}