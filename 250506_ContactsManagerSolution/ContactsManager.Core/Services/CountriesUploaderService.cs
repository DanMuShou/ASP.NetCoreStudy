using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContract;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace ContactsManager.Core.Services;

public class CountriesUploaderService(ICountriesRepository countriesRepository): ICountriesUploaderService
{
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
            if (await countriesRepository.GetCountryByCountryName(countryName) != null)
                continue;
            var countryAddRequest = new Country() { CountryName = countryName };
            await countriesRepository.AddCountry(countryAddRequest);
            countriesInserted += 1;
        }
        return countriesInserted;
    }
}