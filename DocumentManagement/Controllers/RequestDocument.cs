using Azure.Core;
using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                var resualt = await _Request.Get_Approvers(FlowId, UserId);

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
                };
                return errorResponse;
            }
        }

        [HttpPost]
        public async Task<ResponseModel> AddRequest(Request_DTO request)
        {
            try
            {
                await _Request.Add_Request(request);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",

                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = "Bạn thêm luồng phê duyệt chưa thành công",
                };
                return errorResponse;
            }
        }

        [HttpGet]
        public async Task<ResponseModel> GetRequest(int pageNumber , int pageSize)
        {
            try
            {
                var resualt = await _Request.Get_RequestDocument(pageNumber, pageSize);

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

        [HttpDelete("{id}")]
        public async Task<ResponseModel> DeleteRequest(int id)
        {
            try
            {
                await _Request.Delete_Request(id);

                var response = new ResponseModel
                {
                    statusCode = 200,
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
        [HttpPut("{id}/Approval")]
        public async Task<ResponseModel> Approval_Request(ApprovalAction_DTO action)
        {
              try
            {
                await _Request.Approval_Request(action);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Cập nhập thành công.",

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
        [HttpPut("{id}/Reject")]
        public async Task<ResponseModel> Reject_Request(ApprovalAction_DTO action)
        {
            try
            {
                await _Request.Reject_Request(action);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Cập nhập thành công.",

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

    }
}
