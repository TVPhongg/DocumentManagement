using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Application.Services;
using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkLogController : ControllerBase
    {
        private readonly IWorkLogService _workLogService;

        public WorkLogController(IWorkLogService workLogService) 
        {
            _workLogService = workLogService;
        }

        [HttpGet]
        public async Task<ResponseModel> GetAllWorkLog()
        {
            try
            {
                var result = await _workLogService.GetWorkLogAsync();
                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = result,
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new ResponseModel
                {
                    statusCode = 400,
                    message = "Thất bại",
                };
                return response;
            }
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWorkLogById(int userId)
        {
            try
            {
                // Lấy userId từ token JWT
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim == null)
                {
                    return Unauthorized(new ResponseModel
                    {
                        statusCode = 401,
                        message = "Người dùng không hợp lệ."
                    });
                }

                // Chuyển đổi userId từ claim thành int
                if (!int.TryParse(userIdClaim.Value, out int userIdFromToken))
                {
                    return Unauthorized(new ResponseModel
                    {
                        statusCode = 401,
                        message = "Token không hợp lệ."
                    });
                }
                var result = await _workLogService.GetWorkLogsByUserIdAsync(userId);
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "Không tìm thấy dữ liệu chấm công."
                    });
                }

                // Trả về kết quả thành công
                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công",
                    data = result
                });
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nếu cần)
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                });
            }
        }

        [HttpPost]
        public async Task<ResponseModel> CreateWorkLogAsync([FromBody]WorkLog_DTOs workLog)
        {
            try
            {
                await _workLogService.CreateWorkLogAsync(workLog);
                var response = new ResponseModel
                {
                    statusCode = 201,
                    message = "Thành công",
                };
                return response;
            }
            catch(Exception ex)
            {
                var response = new ResponseModel
                {
                    statusCode = 400,
                    message = "Thất bại",
                };
                return response;
            }           
        }

        [HttpPut("{id}")]
        public async Task<ResponseModel> UpdateWorkLogAsync(WorkLog_DTOs workLog, int Id)
        {
            try
            {
                await _workLogService.UpdateWorkLogAsync(workLog, Id);
                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Thành công"
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new ResponseModel
                {
                    statusCode = 400,
                    message = "Thất bại"
                };
                return response;
            }
        }

        [HttpDelete("{id}")]
        public async Task<ResponseModel> DeleteWorkLogAsync(int Id)
        {
            try
            {
                await _workLogService.DeleteWorkLogAsync(Id);
                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Thành công",
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new ResponseModel
                {
                    statusCode = 400,
                    message = "Thất bại"
                };
                return response;
            }
        }
    }
}
