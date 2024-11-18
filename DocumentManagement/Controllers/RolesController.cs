using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Application.Services;
using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ResponseModel> GetRoles()
        {          
            try
            {
                var roles = await _roleService.GetRolesAsync();

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = roles
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

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ResponseModel> GetRoleId(int id)
        {         
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data=role
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = " Không thành công.",
                    data = "Null"
                };
                return errorResponse;
            }
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ResponseModel> CreateRole(Role_Dtos role)
        {
          
            try
            {
                var createdRole = await _roleService.CreateRoleAsync(role);

                var response = new ResponseModel
                {
                    statusCode = 201,
                    message = "Thành công.",
                    data = createdRole
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = " Không thành công.",
                    data = "Null"
                };
                return errorResponse;
            }
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<ResponseModel> UpdateRole(int id, Role_Dtos role)
        {        
            try
            {
                var updated = await _roleService.UpdateRoleAsync(id, role);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Thành công.",
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = " Không thành công.",
                };
                return errorResponse;
            };
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ResponseModel> DeleteRole(int id)
        {         
            try
            {
                var deleted = await _roleService.DeleteRoleAsync(id);

                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Thành công.",
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    message = " Không thành công.",
                };
                return errorResponse;
            }
        }
    }
}
