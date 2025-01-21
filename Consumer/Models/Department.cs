using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE.Maui.CustomControls.Consumer.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int ParentDepartmentId { get; set; }
        public int CompanyId { get; set; }
    }
}
