using DocumentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface IProjectService
    {
        Task <List<Project_DTOs>> GetProjectsAsync ();

        Task<Project_DTOs> GetProjectsByIdAsync(int Id);

        Task UpdateProjectAsync(Project_DTOs project, int Id);

        Task DeleteProjectAsync(int Id);

        Task CreateProjectAsync(Project_DTOs project);
    }
}
