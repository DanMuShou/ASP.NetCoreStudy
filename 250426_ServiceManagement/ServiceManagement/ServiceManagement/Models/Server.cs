using System.ComponentModel.DataAnnotations;

namespace ServiceManagement.Models;

public class Server
{
    public Server()
    {
        var random = new Random();
        var randomNumber = random.Next(0, 2);
        IsOnline = randomNumber != 0;
    }

    public int ServerId { get; set; }
    public bool IsOnline { get; set; }

    [Required(ErrorMessage = "名称不能为空")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "城市不能为空")]
    public string? City { get; set; }
}
