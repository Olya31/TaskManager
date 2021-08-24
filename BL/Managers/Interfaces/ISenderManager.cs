using BL.Models;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Managers.Interfaces
{
    public interface ISenderManager
    {
        Task SendAsync(JobModel job, CancellationToken token);
    }
}
