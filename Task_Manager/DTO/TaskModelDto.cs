namespace Task_Manager.DTO
{
    public class TaskModelDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string CronFormat { get; set; }

        public string Header { get; set; }

        public string Email { get; set; }
    }
}
