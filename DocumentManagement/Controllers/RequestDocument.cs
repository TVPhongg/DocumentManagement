﻿using Azure.Core;
using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Org.BouncyCastle.Asn1.Ocsp;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestDocument : ControllerBase
    {
        private readonly IRequestService _Request;

        public RequestDocument(IRequestService request)
        {
            _Request = request;
        }
        [HttpGet("Approver")]
        public async Task<ResponseModel> GetApprover(int FlowId, int UserId)
        {
            try
            {
                var result = await _Request.Get_Approvers(FlowId, UserId);

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
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = ex.Message,
                };
                return errorResponse;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> AddRequest([FromForm] Request_DTO request)
        {
            // Lấy UserId từ claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized(new ResponseModel
                {
                    statusCode = 401,
                    message = "Người dùng không hợp lệ."
                });
            }

            if (!int.TryParse(userIdClaim.Value, out int userIdInt))
            {
                return Unauthorized(new ResponseModel
                {
                    statusCode = 401,
                    message = "Người dùng không hợp lệ."
                });
            }

            try
            {
                await _Request.Add_Request(request, request.File, userIdInt);

                return StatusCode(201, new ResponseModel
                {
                    statusCode = 201,
                    message = "Thành công."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                    message = $"Bạn thêm luồng phê duyệt chưa thành công: {ex.Message}"
                });
            }
        }



        [Authorize]
        [HttpGet]
        public async Task<ResponseModel> GetRequest()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return new ResponseModel
                {
                    statusCode = 401,
                    message = "Người dùng không hợp lệ."
                };
            }

            // Chuyển đổi userId thành kiểu int
            if (!int.TryParse(userIdClaim.Value, out int userIdInt))
            {
                return new ResponseModel
                {
                    statusCode = 401,
                    message = "Người dùng không hợp lệ."
                };
            }

            try
            {
                var result = await _Request.Get_RequestDocument(userIdInt);

                return new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    statusCode = 400,
                    message = ex.Message,
                    data = "Null"
                };
            }
        }

    [HttpDelete("{id}")]
        public async Task<ResponseModel> DeleteRequest(int id)
        {
            try
            {
                await _Request.Delete_Request(id);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Xóa Thành công.",

                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = ex.Message,
                };
                return errorResponse;
            }
        }

        [HttpGet("search")]
        public async Task<ResponseModel> SearchRequest(string? name, DateTime? StarDate, DateTime? EndDate)
        {
            try
            {
                var resualt = await _Request.Search_Request(name, StarDate,EndDate);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = resualt,

                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = ex.Message,
                    data = "Null",
                };
                return errorResponse;
            }
        }

        [Authorize]
        [HttpPut("{requestId}/Approval")]
        public async Task<ResponseModel> Approval_Request(ApprovalAction_DTO action, int requestId)
        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return new ResponseModel
                {
                    statusCode = 401,
                    message = "Người dùng không hợp lệ."
                };
            }


            if (!int.TryParse(userIdClaim.Value, out int userIdInt))
            {
                return new ResponseModel
                {
                    statusCode = 401,
                    message = "Người dùng không hợp lệ."
                };
            }

            try
            {
                await _Request.Approval_Request(action, userIdInt, requestId);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Phê duyệt thành công.",

                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = ex.Message,
                };
                return errorResponse;
            }
        }

        [Authorize]
        [HttpPut("{requestId}/Reject")]
        public async Task<ResponseModel> Reject_Request(ApprovalAction_DTO action, int requestId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return new ResponseModel
                {
                    statusCode = 401,
                    message = "Người dùng không hợp lệ."
                };
            }


            if (!int.TryParse(userIdClaim.Value, out int userIdInt))
            {
                return new ResponseModel
                {
                    statusCode = 401,
                    message = "Người dùng không hợp lệ."
                };
            }
            try
            {
                await _Request.Reject_Request(action, userIdInt, requestId);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Từ chối phê duyệt thành công.",

                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = ex.Message,
                };
                return errorResponse;
            }
        }

        [HttpGet("download-file")]
        public async Task<IActionResult> DownloadFile([FromQuery] string fileName)
        {
            var filePath = Path.Combine("E:\\DocumentManagement\\DocumentManagement\\File", fileName);

            // Kiểm tra file tồn tại
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File không tồn tại.");
            }

            // Xác định Content Type dựa trên phần mở rộng
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream"; // Mặc định nếu không xác định được loại file
            }

            // Đọc file và trả về kết quả
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType, fileName);
        }
    }
}