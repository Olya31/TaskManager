using ApiServices.ApiService.Interfaces;
using ApiServices.EmailService;
using BL.Managers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Managers
{
    public sealed class SenderManager : ISenderManager
    {
        private readonly IEmailSender _emailSender;
        private readonly IWeatherProcessor _weatherProcessor;
        private readonly IFileManager _fileManager;

        public SenderManager(
            IEmailSender emailSender,
            IWeatherProcessor weatherProcessor,
            IFileManager fileManager)
        {
            _emailSender = emailSender;
            _weatherProcessor = weatherProcessor;
            _fileManager = fileManager;
        }

        public async Task SendAsync(
            string url,
            string email,
            string header,
            CancellationToken token)
        {
            const string Subject = "Weather";

            var weather = await _weatherProcessor.LoadWeatherInformation(url, header, token);
            using var file = _fileManager.ExportToCsv(weather);

            IReadOnlyList<IFormFile> attachments = new List<FormFile>
            {
                new FormFile(file, 0, file.Length, Subject, Subject),
            };

            var message = new Message(new[] { email }, Subject, string.Empty, (IFormFileCollection)attachments);

            await _emailSender.SendEmailAsync(message);
        }
    }
}
