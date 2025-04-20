using System.Drawing;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService(
    IPersonsRepository personsRepository,
    ILogger<PersonsService> logger,
    IDiagnosticContext diagnosticContext
) : IPersonsService
{
    public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
    {
        if (personAddRequest == null)
            ArgumentNullException.ThrowIfNull(personAddRequest);

        //模型验证
        ValidationHelper.ModelValidation(personAddRequest);

        var person = personAddRequest.ToPerson();
        person.PersonId = Guid.NewGuid();

        await personsRepository.AddPerson(person);

        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetAllPersons()
    {
        var persons = await personsRepository.GetAllPersons();
        return persons.Select(person => person.ToPersonResponse()).ToList();
    }

    public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
    {
        if (personId == null)
            return null;

        var person = await personsRepository.GetPersonByPersonId(personId.Value);
        return person?.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
    {
        logger.Log(LogLevel.Information, "服务调用: PersonsService -> GetFilteredPersons");

        List<Person> filterPersonList;
        using (Operation.Time("库中过滤人员所耗时"))
        {
            filterPersonList = searchBy switch
            {
                nameof(PersonResponse.PersonName) => await personsRepository.GetFilteredPersons(temp =>
                    !string.IsNullOrEmpty(temp.PersonName) && temp.PersonName.Contains(searchString ?? string.Empty)
                ),

                nameof(PersonResponse.Email) => await personsRepository.GetFilteredPersons(temp =>
                    !string.IsNullOrEmpty(temp.Email) && temp.Email.Contains(searchString ?? string.Empty)
                ),

                nameof(PersonResponse.DateOfBirth) => await personsRepository.GetFilteredPersons(temp =>
                    temp.DateOfBirth.HasValue
                    && temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString ?? "")
                ),

                nameof(PersonResponse.Gender) => await personsRepository.GetFilteredPersons(temp =>
                    !string.IsNullOrEmpty(temp.Gender) && temp.Gender.Equals(searchString)
                ),

                nameof(PersonResponse.CountryName) => await personsRepository.GetFilteredPersons(temp =>
                    temp.Country != null
                    && !string.IsNullOrEmpty(temp.Country.CountryName)
                    && temp.Country.CountryName.Contains(searchString ?? string.Empty)
                ),

                nameof(PersonResponse.Address) => await personsRepository.GetFilteredPersons(temp =>
                    !string.IsNullOrEmpty(temp.Address) && temp.Address.Contains(searchString ?? string.Empty)
                ),

                _ => await personsRepository.GetAllPersons(),
            };
        }

        //将过滤后的人员列表（filterPersonList）存储到诊断上下文中，以供后续处理或监控使用。
        diagnosticContext.Set("Persons", filterPersonList);
        return filterPersonList.Select(p => p.ToPersonResponse()).ToList();
    }

    public Task<List<PersonResponse>> GetSortPersons(
        List<PersonResponse> allPersons,
        string sortBy,
        SortOrderOptions sortOrder
    )
    {
        logger.Log(LogLevel.Information, "服务调用: PersonsService -> GetSortPersons");

        if (string.IsNullOrEmpty(sortBy))
            return Task.FromResult(allPersons);

        var sortedPersons = (sortBy, sortOrder) switch
        {
            (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase)
                .ToList(),

            (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase)
                .ToList(),

            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.DateOfBirth)
                .ToList(),
            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.DateOfBirth)
                .ToList(),

            (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
            (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.Age)
                .ToList(),

            (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase)
                .ToList(),

            (nameof(PersonResponse.CountryName), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.CountryName), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase)
                .ToList(),

            (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.ReceiveNewsLetters)
                .ToList(),
            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.ReceiveNewsLetters)
                .ToList(),

            _ => allPersons,
        };

        return Task.FromResult(sortedPersons);
    }

    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        logger.Log(LogLevel.Information, "服务调用: PersonsService -> UpdatePerson");

        ArgumentNullException.ThrowIfNull(personUpdateRequest);

        //model validation
        ValidationHelper.ModelValidation(personUpdateRequest);
        //update person
        var targetPerson = await personsRepository.GetPersonByPersonId(personUpdateRequest.PersonId);
        if (targetPerson == null)
            throw new ArgumentException("给出的Person无法找到");

        targetPerson.PersonName = personUpdateRequest.PersonName;
        targetPerson.Email = personUpdateRequest.Email;
        targetPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        targetPerson.Gender = personUpdateRequest.Gender.ToString();
        targetPerson.CountryId = personUpdateRequest.CountryId;
        targetPerson.Address = personUpdateRequest.Address;
        targetPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        var updatedPerson = await personsRepository.UpdatePerson(targetPerson);
        return updatedPerson.ToPersonResponse();
    }

    public async Task<bool> DeletePerson(Guid? personId)
    {
        ArgumentNullException.ThrowIfNull(personId);
        var targetPerson = await personsRepository.GetPersonByPersonId(personId.Value);

        if (targetPerson == null)
            return false;

        //删除该person id的记录
        return await personsRepository.DeletePersonByPersonId(targetPerson.PersonId);
    }

    public async Task<MemoryStream> GetPersonCsv()
    {
        var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true);
        await using var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture));

        // PersonId, PersonName, Email.....
        csvWriter.WriteField(nameof(PersonResponse.PersonName));
        csvWriter.WriteField(nameof(PersonResponse.Email));
        csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
        csvWriter.WriteField(nameof(PersonResponse.Gender));
        csvWriter.WriteField(nameof(PersonResponse.CountryName));
        csvWriter.WriteField(nameof(PersonResponse.Address));
        csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
        await csvWriter.NextRecordAsync(); // 写入表头行
        await csvWriter.NextRecordAsync(); // 空行（换行符）

        var getAllPersonList = await GetAllPersons();

        foreach (var personResponse in getAllPersonList)
        {
            csvWriter.WriteField(personResponse.PersonName);
            csvWriter.WriteField(personResponse.Email);
            csvWriter.WriteField(
                personResponse.DateOfBirth.HasValue ? personResponse.DateOfBirth.Value.ToString("yyyy-MM-dd") : ""
            );
            csvWriter.WriteField(personResponse.Gender);
            csvWriter.WriteField(personResponse.CountryName);
            csvWriter.WriteField(personResponse.Address);
            csvWriter.WriteField(personResponse.ReceiveNewsLetters);
            await csvWriter.NextRecordAsync();
        }
        await csvWriter.FlushAsync();

        //将流的读取指针重置到起始位置(0) 确保后续读取可以从头开始
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task<MemoryStream> GetPersonExcel()
    {
        var memoryStream = new MemoryStream();
        using var excelPackage = new ExcelPackage(memoryStream);
        //新建页面
        var workSheet = excelPackage.Workbook.Worksheets.Add("PersonSheet");
        workSheet.Cells["A1"].Value = "Person Name";
        workSheet.Cells["B1"].Value = "Email";
        workSheet.Cells["C1"].Value = "DateOfBirth";
        workSheet.Cells["D1"].Value = "Age";
        workSheet.Cells["E1"].Value = "Gender";
        workSheet.Cells["F1"].Value = "Country Name";
        workSheet.Cells["G1"].Value = "Address";
        workSheet.Cells["H1"].Value = "ReceiveNewsLetters";

        using var headerCells = workSheet.Cells["A1:H1"];
        headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        headerCells.Style.Font.Bold = true;

        var row = 2;
        var personResponseList = await GetAllPersons();

        foreach (var personResponse in personResponseList)
        {
            workSheet.Cells[row, 1].Value = personResponse.PersonName;
            workSheet.Cells[row, 2].Value = personResponse.Email;
            workSheet.Cells[row, 3].Value = personResponse.DateOfBirth.HasValue
                ? personResponse.DateOfBirth.Value.ToString("yyyy-MM-dd")
                : "";
            workSheet.Cells[row, 4].Value = personResponse.Age;
            workSheet.Cells[row, 5].Value = personResponse.Gender;
            workSheet.Cells[row, 6].Value = personResponse.CountryName;
            workSheet.Cells[row, 7].Value = personResponse.Address;
            workSheet.Cells[row, 8].Value = personResponse.ReceiveNewsLetters;
            row++;
        }
        //自动调整列宽
        workSheet.Cells[$"A1:H{row}"].AutoFitColumns();
        await excelPackage.SaveAsAsync(memoryStream);

        memoryStream.Position = 0;
        return memoryStream;
    }
}
