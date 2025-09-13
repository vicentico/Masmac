using System.ComponentModel.DataAnnotations;

namespace VetUberApp.Domain.Common.Validators;

public static class ModelValidator
{
    public static bool TryValidate<T>(T model, out ICollection<ValidationResult> results) where T : class
    {
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
    }
}