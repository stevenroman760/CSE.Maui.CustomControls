using CSE.Maui.CustomControls.Consumer.Models;
using CSE.Maui.CustomControls.Consumer.Services;
using CSE.Maui.CustomControls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE.Maui.CustomControls.Consumer.Helpers
{
    public class CompanyTreeViewBuilder
    {
        private TreeFolder FindParentDepartment(TreeFolder group, Department department)
        {
            if (group.GroupId == department.ParentDepartmentId)
                return group;

            if (group.Children != null)
            {
                foreach (var currentGroup in group.Children)
                {
                    var search = FindParentDepartment(currentGroup, department);

                    if (search != null)
                        return search;
                }
            }

            return null;
        }

        public TreeFolder GroupData(DataService service)
        {
            var company = service.GetCompany();
            var departments = service.GetDepartments().OrderBy(x => x.ParentDepartmentId);
            var employees = service.GetEmployees();

            var companyGroup = new TreeFolder();
            companyGroup.FolderName = company.CompanyName;

            foreach (var dept in departments)
            {
                var itemGroup = new TreeFolder();
                itemGroup.FolderName = dept.DepartmentName;
                itemGroup.GroupId = dept.DepartmentId;

                // Employees first
                var employeesDepartment = employees.Where(x => x.DepartmentId == dept.DepartmentId);

                foreach (var emp in employeesDepartment)
                {
                    var item = new TreeItem();
                    item.Id = emp.EmployeeId;
                    item.ItemName = emp.EmployeeName;

                    itemGroup.TreeItems.Add(item);
                }

                // Departments now
                if (dept.ParentDepartmentId == -1)
                {
                    companyGroup.Children.Add(itemGroup);
                }
                else
                {
                    TreeFolder parentGroup = null;

                    foreach (var group in companyGroup.Children)
                    {
                        parentGroup = FindParentDepartment(group, dept);

                        if (parentGroup != null)
                        {
                            parentGroup.Children.Add(itemGroup);
                            break;
                        }
                    }
                }
            }

            return companyGroup;
        }
    }
}
