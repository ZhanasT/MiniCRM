using MiniCRMServer.Data;
using System.Text;

namespace MiniCRMServer.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static string GetFullName(this ApplicationUser user)
        {
            var sb = new StringBuilder(5);
            sb.Append(user.FirstName);
            sb.Append(' ');
            sb.Append(user.LastName);
            sb.Append(' ');
            sb.Append(user.Patronymic);
            return sb.ToString();
        }
    }
}
