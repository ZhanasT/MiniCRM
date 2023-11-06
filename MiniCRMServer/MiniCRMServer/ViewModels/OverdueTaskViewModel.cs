namespace MiniCRMServer.ViewModels
{
    public class OverdueTaskViewModel
    {
        public int Id { get; set; }
        public string EmployeeFullName { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeadLine { get; set; }
        public int CompetionRate { get; set; }
        public int DaysOverdue { get; set; }
    }
}
