using ApiServices.ApiService.Interfaces;
using ApiServices.EmailService;
using BL.Managers.Interfaces;
using BL.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
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

        public async Task SendAsync(JobModel job, CancellationToken token)
        {
            const string Subject = "Weather";

            var weather = await _weatherProcessor.LoadWeatherInformation(job.Url, job.Header, token);
            using var stream = GenerateStreamFromString(weather);

            var file = new FormFile(stream, 0, stream.Length, Subject, Subject);

            var message = new Message(new[] { job.Email }, job.Name, job.Description, file);

            await _emailSender.SendEmailAsync(message);
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
