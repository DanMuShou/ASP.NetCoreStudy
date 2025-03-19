using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelValidationsExample.Model;

namespace ModelValidationsExample.Controllers;

[Controller]
public class HomeController : Controller
{
    [Route("register")]
    public IActionResult Home(Person person)
    {
        if (!ModelState.IsValid)
        {
            // List<string> errorList = [];
            // foreach (var value in ModelState.Values)
            // {
            //     foreach (var error in value.Errors)
            //     {
            //         errorList.Add(error.ErrorMessage);
            //     }
            // }
            // var errors = string.Join("\n", errorList);

          var errors =  string.Join("\n", 
              ModelState.Values.
                  SelectMany(values => values.Errors).
                  Select(error => error.ErrorMessage));
            
            return BadRequest("The person required information is not filled\n" + errors);
        }
        
        return Content($"{person}");
    }
}