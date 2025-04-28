namespace ServiceManagement.Models;

public class CitiesRepository
{
    private static readonly List<string> Cities =
    [
        "Toronto",
        "Montreal",
        "Ottawa",
        "Calgary",
        "Halifax",
    ];

    public static List<string> GetCities() => Cities;
}
