
namespace SoftUni
{
    using SoftUni.Data;
    using SoftUni.Models;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            string employeesInfo = GetEmployeesFullInformation(context);
            Console.WriteLine(employeesInfo);
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    FullInfo = $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}"
                })
                .ToList();
            foreach (var item in employees)
            {
                sb.AppendLine(item.FullInfo);
            }
            return sb.ToString().TrimEnd();
        }
    }
}
