using DocumentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface IWorkLogService
    {
        Task <List<WorkLog_DTOs>> GetWorkLogAsync ();

        Task<List<WorkLog_DTOs>> GetWorkLogsByUserIdAsync(int userId);

        Task CreateWorkLogAsync(WorkLog_DTOs workLog);

        Task DeleteWorkLogAsync (int id);

        Task UpdateWorkLogAsync (WorkLog_DTOs workLog, int Id);
    }
}
