using DocumentManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportExcelController : ControllerBase
    {
        private readonly IExportExcelService _exportExcelService;

        public ExportExcelController(IExportExcelService exportExcelService)
        {
            _exportExcelService = exportExcelService;
        }
        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportExcel()
        {
            var fileContents = await _exportExcelService.ExportExcelAsync();

            var fileName = "SalaryReport.xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(fileContents, contentType, fileName);
        }
    }
}
