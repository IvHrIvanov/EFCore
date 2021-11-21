
namespace SoftUni
{
    using Microsoft.EntityFrameworkCore;
    using SoftUni.Data;
    using SoftUni.Models;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            string employeesInfo = GetEmployeesInPeriod(context);
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
                .OrderBy(e => e.FirstName)
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
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                });
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            Address address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            var employee = context.Employees
                .FirstOrDefault(x => x.LastName == "Nakov");

            employee.Address = address;
            context.SaveChanges();
            var allEmployee = context.Employees
                .OrderByDescending(x => x.AddressId)
                .Select(x => x.Address.AddressText)
                .Take(10)
                .ToList();

            return string.Join(Environment.NewLine, allEmployee);
        }
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(employee => employee.EmployeesProjects.Any(project => project.Project.StartDate.Year >= 2001 && project.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    EmpFristName = e.FirstName,
                    EmpLastName = e.LastName,
                    MenFirstName = e.Manager.FirstName,
                    MenLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(x => new
                    {

                        ProjectName = x.Project.Name,
                        StartDate = x.Project.StartDate,
                        EndDate = x.Project.EndDate
                    })
                })
                .Take(10)
                .ToList();

            foreach (var e in employees)
            {
               
                sb.AppendLine($"{e.EmpFristName} {e.EmpLastName} - Manager: {e.MenFirstName} {e.MenLastName}");
                foreach (var item in e.Projects)
                {
                    var endDate = item.EndDate.HasValue ? item.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", new CultureInfo("en-US")) : "not finished";
                    sb.AppendLine($"--{item.ProjectName} - " +
                        $"{item.StartDate.ToString("M/d/yyyy h:mm:ss tt", new CultureInfo("en-US"))} - " +
                        $"{endDate}");
                }
            }
            return sb.ToString().TrimEnd();
        }
    }
}