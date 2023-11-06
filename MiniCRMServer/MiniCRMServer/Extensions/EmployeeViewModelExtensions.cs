using MiniCRMServer.ViewModels;

namespace MiniCRMServer.Extensions
{
    public static class EmployeeViewModelExtensions
    {
        public static string[] GetSplitFullName(this EmployeeViewModel employeeViewModel)
        {
            return employeeViewModel.FullName.Split(' ');
        }
    }
}
