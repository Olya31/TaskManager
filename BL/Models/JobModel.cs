using BL.Extantions;
using Entities.Models;

namespace BL.Models
{
    public sealed class JobModel
    {
        public JobModel(TaskModel taskModel)
        {
            this.Id = taskModel.Id;
            this.Name = taskModel.Name;
            this.Description = taskModel.Description;
            this.Url = taskModel.Url;
            this.Email = taskModel.Email;
            this.Cron = taskModel.CronFormat.ConvertToCronModel();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Email { get; set; }

        public CronModel Cron { get; set; }
    }
}
