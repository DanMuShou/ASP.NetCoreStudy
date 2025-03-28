using ServiceContracts;

namespace Services;

public class CitiesService : ICitiesService, IDisposable
{
    private List<string> _cities;

    public Guid ServiceInstanceId { get; }

    public CitiesService()
    {
        ServiceInstanceId = Guid.NewGuid();
        _cities =
        [
            "London",
            "Paris",
            "Berlin",
            "Tokyo",
            "New York",
            "Rome",
            "Moscow",
            "Beijing",
            "Cairo",
        ];
    }

    public List<string> GetCities()
    {
        return _cities;
    }

    //添加逻辑来关闭数据库连接
    public void Dispose() { }
}
