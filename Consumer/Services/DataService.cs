using CSE.Maui.CustomControls.Consumer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE.Maui.CustomControls.Consumer.Services
{
    public class DataService
    {
        public Company GetCompany()
        {
            return new Company()
            {
                CompanyId = 1,
                CompanyName = "TC Solutions"
            };
        }

        public IEnumerable<Department> GetDepartments()
        {
            return new List<Department>()
            {
                new Department() { CompanyId = 1, DepartmentId = 1, DepartmentName = "Software Development", ParentDepartmentId = -1 },
                new Department() { CompanyId = 1,  DepartmentId = 2, DepartmentName = "Marketing", ParentDepartmentId = -1 },
            };
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return new List<Employee>()
            {
                new Employee() { EmployeeId = 1, EmployeeName = "Valuable Coder", DepartmentId = 1 },
                new Employee() { EmployeeId = 2, EmployeeName = "Stupid Bean Counter", DepartmentId = 2 },
            };
        }
    }
}
