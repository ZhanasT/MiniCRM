using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MiniCRMServer.Data
{
    #nullable disable
    public class ApplicationUser : IdentityUser
    {
        public string Position { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();

    }
}
