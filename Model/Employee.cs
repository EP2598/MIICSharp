using System;
using System.Collections.Generic;
using System.Text;

namespace LinqImplementation.Model
{
    public class Employee
    {
        public long EmpId { get; set; }
        public string EmpName { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime StartDate { get; set; }
        public long DivisionId { get; set; }
        public long RoleId { get; set; }
        public long RefOfficeId { get; set; }
        public char IsActive { get; set; }
    }
}
