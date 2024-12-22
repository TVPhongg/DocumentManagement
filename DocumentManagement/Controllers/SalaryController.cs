using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Application.Services;
using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService _salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        [HttpPut("{id}")]
        public async Task<ResponseModel> UpdateSalaryAsync([FromBody]UpdateSalary_DTOs salary, int id)
        {
            try
            {
                await _salaryService.UpdateSalaryAsync(salary, id);
                var response = new ResponseModel
                 {
                     statusCode = 204,
                     message = "Cập nhật bản lương thành công",
                 };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Cập nhật bản lương thất bại"
                };
                return errorResponse;
            }

        }
        [HttpPost("{id}")]
        public async Task<ResponseModel> InsertSalaryAsync([FromBody] UpdateSalary_DTOs salary, int id)
        {
            try
            {
                await _salaryService.InsertSalaryAsync(salary, id);
                var response = new ResponseModel
                {
                    statusCode = 201,
                    message = "Thêm bản lương thành công",
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Thêm bản lương thất bại"
                };
                return errorResponse;
            }
        }
    }
}
