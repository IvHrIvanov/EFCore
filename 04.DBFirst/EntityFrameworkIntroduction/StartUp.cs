
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
            string employeesInfo = GetEmployeesWithSalaryOver50000(context);
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
            foreach (var employee in employees)
            {
                sb.AppendLine(employee.FullInfo);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e=>e.FirstName)
                .Select(e => new
                {
                    EmployeeSalaryOver5000 = $"{e.FirstName} - {e.Salary:f2}"
                }).ToList();
            foreach (var emloyee in employees)
            {
                sb.AppendLine(emloyee.EmployeeSalaryOver5000);
            }
            return sb.ToString().TrimEnd();
        }
    }
}