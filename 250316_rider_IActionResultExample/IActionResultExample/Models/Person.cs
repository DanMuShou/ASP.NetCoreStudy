using System.ComponentModel.DataAnnotations;

namespace IActionResultExample.Models;
public class Person
{
    [Required(ErrorMessage = "Person name can't be in valid")] 
    public string? PersonName { get; set; }
    
    public string? Email   { get; set;}
    public string? Password { get; set;}
    
    public override string ToString()
    {
        return $"PersonName : {PersonName} \n" + $"Email : {Email} \n" + $"Password : {Password}";
    }
}