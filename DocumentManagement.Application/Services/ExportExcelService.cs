using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Context;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Services
{
    public class ExportExcelService : IExportExcelService
    {
        private readonly MyDbContext _dbContext;

        public ExportExcelService(MyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<byte[]> ExportExcelAsync()
        {
            // Lấy dữ liệu từ bảng User và Salary
            var data = await _dbContext.User
                .Include(u => u.Salary) // Join với bảng Salary
                .Select(u => new ExportExcel_DTOs
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Address = u.Address,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Gender = u.Gender,
                    BaseSalary = u.Salary != null ? u.Salary.BaseSalary : 0,
                    Allowances = u.Salary != null ? u.Salary.Allowances : 0,
                    Bonus = u.Salary != null ? u.Salary.Bonus : 0,
                    TotalSalary = u.Salary != null ? u.Salary.TotalSalary : 0,
                    ActualWorkingHours = u.Salary != null ? u.Salary.ActualWorkingHours : 0,
                    Month = u.Salary.Month,
                    Year = u.Salary.Year,
                }).ToListAsync();

            // Gọi hàm để tạo file Excel từ dữ liệu
            return GenerateExcelFile(data);
        }

        private byte[] GenerateExcelFile(List<ExportExcel_DTOs> excel_DTOs)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Salary Report");

                // Tạo tiêu đề
                var headers = new string[]
                {
                    "STT","First Name", "Last Name", "Address", "Email", "Phone Number",
                    "Gender", "Month", "Year", "Base Salary",
                    "Allowances", "Bonus", "Total Salary", "Actual Working Hours", 
                };

                // Ghi tiêu đề vào Excel
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }

                // Ghi dữ liệu vào Excel
                int row = 2;
                int stt = 1;
                foreach (var item in excel_DTOs)
                {
                    worksheet.Cells[row, 1].Value = stt++;
                    worksheet.Cells[row, 2].Value = item.FirstName;
                    worksheet.Cells[row, 3].Value = item.LastName;
                    worksheet.Cells[row, 4].Value = item.Address;
                    worksheet.Cells[row, 5].Value = item.Email;
                    worksheet.Cells[row, 6].Value = item.PhoneNumber;
                    worksheet.Cells[row, 7].Value = item.Gender;
                    worksheet.Cells[row, 8].Value = item.Month;
                    worksheet.Cells[row, 9].Value = item.Year;
                    worksheet.Cells[row, 10].Value = item.BaseSalary;
                    worksheet.Cells[row, 10].Style.Numberformat.Format = "#,##0.00 ₫";

                    worksheet.Cells[row, 11].Value = item.Allowances;
                    worksheet.Cells[row, 11].Style.Numberformat.Format = "#,##0.00 ₫";

                    worksheet.Cells[row, 12].Value = item.Bonus;
                    worksheet.Cells[row, 12].Style.Numberformat.Format = "#,##0.00 ₫";

                    worksheet.Cells[row, 13].Value = item.TotalSalary;
                    worksheet.Cells[row, 13].Style.Numberformat.Format = "#,##0.00 ₫";
                    worksheet.Cells[row, 14].Value = item.ActualWorkingHours;

                    // Áp dụng viền cho từng ô
                    for (int col = 1; col <= headers.Length; col++)
                    {
                        worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }

                    row++;
                }

                // Tạo viền cho phạm vi dữ liệu (cả bảng)
                var dataRange = worksheet.Cells[1, 1, row - 1, headers.Length];
                dataRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                // AutoFit cột để vừa dữ liệu
                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

    }
}
