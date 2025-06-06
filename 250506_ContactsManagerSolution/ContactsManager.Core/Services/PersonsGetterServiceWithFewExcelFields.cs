﻿using System.Drawing;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ContactsManager.Core.Services;

public class PersonsGetterServiceWithFewExcelFields(PersonsGetterService personsGetterService) : IPersonsGetterService
{
    public async Task<List<PersonResponse>> GetAllPersons() => await personsGetterService.GetAllPersons();

    public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId) =>
        await personsGetterService.GetPersonByPersonId(personId);

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString) =>
        await personsGetterService.GetFilteredPersons(searchBy, searchString);

    public Task<MemoryStream> GetPersonCsv() => personsGetterService.GetPersonCsv();

    public async Task<MemoryStream> GetPersonExcel()
    {
        var memoryStream = new MemoryStream();
        using var excelPackage = new ExcelPackage(memoryStream);
        //新建页面
        var workSheet = excelPackage.Workbook.Worksheets.Add("PersonSheet");
        workSheet.Cells["A1"].Value = "Person Name";
        workSheet.Cells["B1"].Value = "Age";
        workSheet.Cells["C1"].Value = "Gender";

        using var headerCells = workSheet.Cells["A1:C1"];
        headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        headerCells.Style.Font.Bold = true;

        var row = 2;
        var personResponseList = await GetAllPersons();

        foreach (var personResponse in personResponseList)
        {
            workSheet.Cells[row, 1].Value = personResponse.PersonName;
            workSheet.Cells[row, 2].Value = personResponse.Age;
            workSheet.Cells[row, 3].Value = personResponse.Gender;
            row++;
        }
        //自动调整列宽
        workSheet.Cells[$"A1:C{row}"].AutoFitColumns();
        await excelPackage.SaveAsAsync(memoryStream);

        memoryStream.Position = 0;
        return memoryStream;
    }
}
