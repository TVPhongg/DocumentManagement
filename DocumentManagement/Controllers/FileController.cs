using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Application.Services;
using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpGet("{folderId}")]
        public async Task<ResponseModel> GetAll(int folderId)
        {
            try
            {
                var resuilt = await _fileService.GetAllFile(folderId);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = resuilt
                };
                return response;
            }
            catch (UnauthorizedAccessException)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn không có quyền truy cập vào tài nguyên này"
                };
                return errorResponse;
            }
            catch (Exception ex)
            {;

                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal Server Error."
                };
                return errorResponse;
            }
        }
        [HttpDelete("{id}")]
        public async Task<ResponseModel> DeleteFile(int id, int currentUserId)
        {
            try
            {
                await _fileService.DeleteFile(id, currentUserId);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Bạn xóa file thành công."
                };
                return response;
            }
            catch (UnauthorizedAccessException)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn xóa file không thành công."
                };
                return errorResponse;
            }
            catch (Exception ex)
            {

                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal Server Error."
                };
                return errorResponse;
            }
        }
        [HttpPatch("{id}")]
        public async Task<ResponseModel> UpdateFile([FromBody] string newName, int id, int currentUserId)
        {
            try
            {
                await _fileService.UpdateFile(newName, id, currentUserId);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Bạn sửa tên file thành công."
                };

                return response;
            }
            catch (UnauthorizedAccessException)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn sửa tên file không thành công."
                };
                return errorResponse;
            }
            catch (Exception ex)
            {

                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal Server Error."
                };
                return errorResponse;
            }
        }
        [HttpPost]
        public async Task<ResponseModel> AddFile([FromForm] List<IFormFile> files, [FromForm] int foldersId, [FromForm] int userId)
        {
            try
            {
                var fileDto = new File_DTOs
                {
                    FoldersId = foldersId,
                    UserId = userId
                };

                await _fileService.AddFiles(fileDto, files);

                var response = new ResponseModel
                {
                    statusCode = 201,
                    message = "Bạn tải file lên thành công."
                };
                return response;
            }
            catch (UnauthorizedAccessException)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn tải file lên không thành công."
                };
                return errorResponse;
            }
            catch (Exception)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau."
                };
                return errorResponse;
            }
        }
        [HttpPost("Share")]
        public async Task<ResponseModel> Sharefolders(List<FilePermissionDTOs> filePermissions)
        {
            try
            {
                await _fileService.ShareFile(filePermissions);

                var response = new ResponseModel
                {
                    statusCode = 201,
                    message = "Bạn chia sẻ file thành công."
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
        [HttpGet("Search")]
        public async Task<ResponseModel> SearchFile(string searchTerm)
        {
            try
            {
                var result = await _fileService.SearchFile(searchTerm);

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

    }
}
