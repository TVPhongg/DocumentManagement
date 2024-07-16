using Azure;
using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Entities;
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
        [HttpPatch("/folders/{id}")]
        public async Task<ResponseModel> UpdateFolder(Folder_DTOs Folder, int id)
        {
            try
            {
                await _folderService.UpdateFolder(Folder, id);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Bạn sửa tên thư mục thành công."
                };

                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn sửa tên thư mục không thành công."
                };
                return errorResponse;
            }

        }
        [HttpPost("/folders")]
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
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn thêm thư mục không thành công."
                };
                return errorResponse;
            }
        }
        [HttpDelete("/folders/{id}")]
        public async Task<ResponseModel> DeleteFolders(int id)
        {
            try
            {
                await _folderService.DeleteFolder(id);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Bạn xóa thư mục thành công."
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Bạn xóa không thành công."
                };
                return errorResponse;
            }
        }
       
    }
}
