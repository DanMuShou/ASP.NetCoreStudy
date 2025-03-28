using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelValidationsExample.Model;

namespace ModelValidationsExample.Custom;

public class PersonModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var person = new Person();

        //姓和名
        if (bindingContext.ValueProvider.GetValue("FirstName").Length > 0)
        {
            person.PersonName = bindingContext.ValueProvider.GetValue("FirstName").First();

            if (bindingContext.ValueProvider.GetValue("LastName").Length > 0)
            {
                person.PersonName += " " + bindingContext.ValueProvider.GetValue("LastName").First();
            }
        }

        //Email
        if (bindingContext.ValueProvider.GetValue("Email").Length > 0)
        {
            person.Email = bindingContext.ValueProvider.GetValue("Email").First();
        }

        //密码
        if (bindingContext.ValueProvider.GetValue("Password").Length > 0)
        {
            person.Password = bindingContext.ValueProvider.GetValue("Password").First();
        }

        //确认密码
        if (bindingContext.ValueProvider.GetValue("ConfirmPassword").Length > 0)
        {
            person.ConfirmPassword = bindingContext.ValueProvider.GetValue("ConfirmPassword").First();
        }

        //电话
        if (bindingContext.ValueProvider.GetValue("Phone").Length > 0)
        {
            person.Phone = bindingContext.ValueProvider.GetValue("Phone").First();
        }

        //价格
        if (bindingContext.ValueProvider.GetValue("Price").Length > 0)
        {
            person.Price = double.Parse(bindingContext.ValueProvider.GetValue("Price").First());
        }

        if (bindingContext.ValueProvider.GetValue("DateOfBrith").Length > 0)
        {
            person.DateOfBrith = DateTime.Parse(bindingContext.ValueProvider.GetValue("DateOfBrith").First());
        }

        // if (bindingContext.ValueProvider.GetValue("FromDate").Length > 0)
        // {
        //     person.FromDate = DateTime.Parse(bindingContext.ValueProvider.GetValue("FromDate").First());
        // }
        //
        // if (bindingContext.ValueProvider.GetValue("ToDate").Length > 0)
        // {
        //     person.ToDate = DateTime.Parse(bindingContext.ValueProvider.GetValue("ToDate").First());
        // }

        bindingContext.Result = ModelBindingResult.Success(person);
        return Task.CompletedTask;
    }
}