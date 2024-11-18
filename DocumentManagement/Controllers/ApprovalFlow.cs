using Azure.Core;
using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Application.Services;
using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalFlow : ControllerBase
    {
        private readonly IFlowService _flowService;

        public ApprovalFlow(IFlowService flowService)
        {
            _flowService = flowService;
        }


        [HttpPost]
        public async Task<ResponseModel> AddFlow(ApprovalFlow_DTO request)
        {
            try
            {
                await _flowService.AddFlow(request);

                var response = new ResponseModel
                {
                    statusCode = 201,
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
        public async Task<ResponseModel> GetFlows()
        {
            try
            {
                var resualt = await _flowService.Get_ApprovalFlows();

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = resualt
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    data = "Null",
                };
                return errorResponse;
            }
        }

        [HttpGet("{id}")]
        public async Task<ResponseModel> GetFlow(int id)
        {
            try
            {
                var resualt = await _flowService.Get_ApprovalFlow(id);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = resualt
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    data = "Null",
                };
                return errorResponse;
            }
        }

        [HttpPut("{id}")]
        public async Task<ResponseModel> UpdateFlow(int id, ApprovalFlow_DTO request)
        {
            try
            {
                await _flowService.Update_ApprovalFlow(id, request);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Cập nhập thành công."
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = "Cập nhật thất bại.",
                };
                return errorResponse;
            }
        }
        [HttpDelete("{id}")]
        public async Task<ResponseModel> DeleteFlow(int id)
        {
            try
            {
                await _flowService.Delete_ApprovalFlow(id);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Xóa thành công."
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = "Xóa thất bại.",
                };
                return errorResponse;
            }
        }

        [HttpGet("search")]
        public async  Task<ResponseModel> SearchFlow(string? name)
        {
            try
            {
                var resualt = await _flowService.Seach_ApprovalFlows(name);      

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

     
    }
}
