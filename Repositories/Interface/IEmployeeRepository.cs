using LinqImplementation.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinqImplementation.Repositories.Interface
{
    interface IEmployeeRepository
    {
        void AddEmployee(Employee emp, bool IsError = false, int step = 1);
        void EditEmployee(bool IsError = false, bool IsActivation = false);
        void DeleteEmployee(bool IsError = false);
        void PrintData(string dataType = "Employee");
    }
}
