using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.Helpers;

public static class ValidationHelper
{
    //internal : 只能在程序集内部使用
    internal static void ModelValidation(object obj)
    {
        //模型验证
        var validationContext = new ValidationContext(obj);
        var validationResults = new List<ValidationResult>();
        //true - 验证所有属性
        var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

        if (!isValid)
        {
            throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
        }
    }
}
