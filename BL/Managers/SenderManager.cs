using ApiServices.ApiService.Interfaces;
using ApiServices.EmailService;
using BL.Managers.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Managers
{
    public sealed class SenderManager : ISenderManager
    {
        private readonly IEmailSender _emailSender;
        private readonly IWeatherProcessor _weatherProcessor;

        public SenderManager(
            IEmailSender emailSender,
            IWeatherProcessor weatherProcessor)
        {
            _emailSender = emailSender;
            _weatherProcessor = weatherProcessor;
        }

        public async Task SendAsync(string url, string email, CancellationToken token)
        {
            var weather = await _weatherProcessor.LoadWeatherInformation(url, string.Empty); // TODO add headers
            // 
        }
    }
}
