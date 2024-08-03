namespace P2Api.Models
{
    public class Tarefa
    {
        public int id { get; set; }
        public int createdByUser { get; set; }
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
    }
}
