using MiniCRMServer.Data;

namespace MiniCRMServer.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeadLine { get; set; }
        public int CompetionRate { get; set; }
    }
}
