using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly ICountriesRepository _countriesRepository;

    public CountriesService(ICountriesRepository countriesRepository)
    {
        _countriesRepository = countriesRepository;
    }

    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
    {
        //传入参数校验
        ArgumentNullException.ThrowIfNull(countryAddRequest);
        //参数校验
        if (countryAddRequest.CountryName == null)
            throw new ArgumentException(null, nameof(countryAddRequest));

        //请求 -> 国家实体
        var country = countryAddRequest.ToCountry();
        //生成GuidId
        country.CountryId = Guid.NewGuid();
        //添加国家
        if ((await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName)) != null)
            throw new ArgumentException("给出的国家名称已经添加");

        //保存数据
        await _countriesRepository.AddCountry(country);

        //返回国家响应
        return country.ToCountryResponse();
    }

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        var getAllCountryList = await _countriesRepository.GetAllCountries();
        return getAllCountryList.Select(country => country.ToCountryResponse()).ToList();
    }

    public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
    {
        if (countryId == null)
            return null;
        var country = await _countriesRepository.GetCountryById(countryId.Value);
        return country?.ToCountryResponse();
    }

    public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
    {
        var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        var countriesInserted = 0;

        using var excelPackage = new ExcelPackage(memoryStream);
        var workSheet = excelPackage.Workbook.Worksheets["Countries"];
        var rowCount = workSheet.Dimension.Rows;

        for (var row = 2; row <= rowCount; row++)
        {
            var cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);
            if (string.IsNullOrEmpty(cellValue))
                continue;

            var countryName = cellValue;
            if (await _countriesRepository.GetCountryByCountryName(countryName) != null)
                continue;
            var countryAddRequest = new Country() { CountryName = countryName };
            await _countriesRepository.AddCountry(countryAddRequest);
            countriesInserted += 1;
        }
        return countriesInserted;
    }
}
