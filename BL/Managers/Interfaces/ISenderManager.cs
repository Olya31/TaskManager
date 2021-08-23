using System.Threading;
using System.Threading.Tasks;

namespace BL.Managers.Interfaces
{
    public interface ISenderManager
    {
        Task SendAsync(string url,
            string email,
            string header,
            CancellationToken token);
    }
}
