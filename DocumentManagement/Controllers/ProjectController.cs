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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _project;

        public ProjectController(IProjectService project) 
        {
            _project = project;
        }
        [HttpGet]
        public async Task<ResponseModel> GetAllProject()
        {
            try
            {
                var result = await _project.GetProjectsAsync();

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = result
                };
                return response;
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

        [HttpGet("{id}")]
        public async Task<ResponseModel> GetProjectById(int id)
        {         
            try
            {
                var result = await _project.GetProjectsByIdAsync(id);
                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = result
                };
                return response;
            }
            catch(Exception ex)
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
        public async Task<ResponseModel> CreateProject( Project_DTOs project_DTOs)
        {
            try
            {
                await _project.CreateProjectAsync(project_DTOs);
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
                    statusCode = 500,
                    message = "Internal Server Error."
                };
                return errorResponse;
            }
        }

        [HttpPut("{id}")]
        public async Task<ResponseModel> UpdateProject(Project_DTOs project_DTOs, int id)
        {
            try
            {
                await _project.UpdateProjectAsync(project_DTOs, id);
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
                    statusCode = 500,
                    message = "Internal Server Error."
                };
                return errorResponse;
            }
        }

        [HttpDelete("{id}")]
        public async Task<ResponseModel> DeleteProject(int id)
        {
            try
            {
                await _project.DeleteProjectAsync(id);
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
                    statusCode = 500,
                    message = "Internal Server Error."
                };
                return errorResponse;
            }
        }
    }
}
