using System.ComponentModel.DataAnnotations;

namespace AccountProvider.Helpers;

public class ValidModel
{
    public static bool IsValid(object? model)
    {
        if (model != null)
        {
            ValidationContext context = new ValidationContext(model);
            List<ValidationResult> errors = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, errors, true);
        }
        return false;
    }
}
