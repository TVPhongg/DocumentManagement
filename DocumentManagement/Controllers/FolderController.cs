using Azure;
using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }
        [HttpGet]
        public async Task<ResponseModel> GetAll(int currentUserId)
        {
            try
            {
                var result = await _folderService.GetAllFolder(currentUserId);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = result
                };

                return response;
            }
            catch (UnauthorizedAccessException)  
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn không có quyền truy cập vào tài nguyên này."
                };
                return errorResponse;
            }
            catch (Exception ex) 
            {

                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    message = ex.Message
                };
                return errorResponse;
            }
        }

        [HttpPatch("{id}")]
        public async Task<ResponseModel> UpdateFolder([FromBody] string newName, int id, int currentUserId)
        {
            try
            {
                await _folderService.UpdateFolder(newName, id,currentUserId);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Bạn sửa tên thư mục thành công."
                };

                return response;
            }
            catch (UnauthorizedAccessException)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn Không có quyền thực hiện hành động này."
                };
                return errorResponse;
            }
            catch (Exception ex)
            {

                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    message = ex.Message
                };
                return errorResponse;
            }

        }
        [HttpPost]
        public async Task<ResponseModel> Addfolders (Folder_DTOs Folder)
        {
            try
            {
                await _folderService.AddFolder(Folder);

                var response = new ResponseModel
                {
                    statusCode = 201,
                    message = "Bạn thêm thư mục thành công."
                };
                return response;
            }
            catch (UnauthorizedAccessException)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn Không có quyền thực hiện hành động này."
                };
                return errorResponse;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    message = ex.Message
                };
                return errorResponse;
            }
        }
        [HttpDelete("{id}")]

        public async Task<ResponseModel> DeleteFolders(int id, int currentUserId)
        {
            try
            {
                await _folderService.DeleteFolder(id, currentUserId);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Bạn xóa thư mục thành công."
                };
                return response;
            }
            catch (UnauthorizedAccessException)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn không có quyền thực hiện hành động này."
                };
                return errorResponse;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi để theo dõi (tùy chọn)
                // _logger.LogError(ex, "An unexpected error occurred.");

                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    message = ex.Message
                };
                return errorResponse;
            }
        }
        [HttpGet("Search")]
        public async Task<ResponseModel> SearchFolder(string searchTerm)
        {
            try
            {
                var result = await _folderService.SearchFolder(searchTerm);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = result
                };

                return response;
            }
            catch (UnauthorizedAccessException)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn không có quyền thực hiện hành động này."
                };
                return errorResponse;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    message = ex.Message
                };
                return errorResponse;
            }
        }
        [HttpPost("Share")]
        public async Task<ResponseModel> Sharefolders(List<FolderPermissionDTOs> folderPermission)
        {
            try
            {
                await _folderService.ShareFolder(folderPermission);

                var response = new ResponseModel
                {
                    statusCode = 201,
                    message = "Bạn chia sẻ thư mục thành công."
                };
                return response;
            }
            catch (UnauthorizedAccessException)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn Không có quyền thực hiện hành động này."
                };
                return errorResponse;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    message = ex.Message
                };
                return errorResponse;
            }
        }
    }
}
