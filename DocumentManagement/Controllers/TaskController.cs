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
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService) 
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ResponseModel> GetAllTask()
        {
            try
            {
                var result = await _taskService.GetTasksAsync();
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
                    statusCode = 500,
                    message = "Internal Server Error."
                };
                return errorResponse;
            }
        }

        [HttpGet("{id}/details")]
        public async Task<ResponseModel> GetTaskById(int id)
        {
            try
            {
                var result = await _taskService.GetTaskByIdAsync(id);
                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công",
                    data = result,
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

        [HttpPost]
        public async Task<ResponseModel> CreateTaskAsync(Task_DTOs task)
        {
            try
            {
                await _taskService.CreateTaskAsync(task);
                var response = new ResponseModel
                {
                    statusCode = 201,
                    message = "Thành công"
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
        public async Task<ResponseModel> UpdateTaskAsync(Task_DTOs task, int id)
        {
            try
            {
                await _taskService.UpdateTaskAsync(task, id);
                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Thành công"
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

        [HttpDelete("{id}")]
        public async Task<ResponseModel> DeleteTaskAsync(int id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                var response = new ResponseModel
                {
                    statusCode = 204,
                    message = "Thành công"
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

        [HttpGet("{projectId}")]
        public async Task<ResponseModel> GetTaskByProjectId(int projectId)
        {
            try
            {
                var result = await _taskService.GetTasksByProjectAsync(projectId);
                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công",
                    data = result,
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
