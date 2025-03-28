using _250324_rider_ViewComponentsExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace _250324_rider_ViewComponentsExample.ViewComponents;

[ViewComponent]
public class GridViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PersonGridModel? gridModel)
    {
        if (gridModel == null)
        {
            gridModel = new PersonGridModel()
            {
                GridTitle = "Person List",
                Persons =
                [
                    new Person() { Name = "John", JobTitle = "Developer" },
                    new Person() { Name = "Jane", JobTitle = "Designer" },
                    new Person() { Name = "Jim", JobTitle = "Manager" },
                ],
            };
        }
        //Views/Shared/Components/Grid/GridViewComponent.cshtml
        return View("Sample", gridModel);
    }
}
