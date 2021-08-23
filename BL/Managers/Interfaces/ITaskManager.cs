using Entities.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Managers.Interfaces
{
    public interface ITaskManager
    {
        Task<int> AddTaskAsync(TaskModel task, CancellationToken token);

        Task<int> DeleteTaskAsync(int id, CancellationToken token);

        Task<int> UpdateAsync(TaskModel task, CancellationToken token);

        Task<List<TaskModel>> GetAllTaskAsync(CancellationToken token);

        Task<TaskModel> GetTaskByIdAsync(int id, CancellationToken token);
    }
}
