namespace Entities.Models
{
    public class TaskModel
    {
        public TaskModel(int Id, 
            string Name, 
            string Description, 
            string Url, 
            string CronFormat,
            string Email,
            string Header)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.Url = Url;
            this.CronFormat = CronFormat;
            this.Email = Email;
            this.Header = Header;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Email { get; set; }

        public string CronFormat { get; set; }

        public string Header { get; set; }
    }
}
