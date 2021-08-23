using System.Net.Http;
using System.Threading.Tasks;

namespace ApiServices.ApiService.Interfaces
{
    public interface IWeatherProcessor
    {
        Task<string> LoadWeatherInformation(string url, string header);
    }
}
