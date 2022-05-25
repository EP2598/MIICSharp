using LinqImplementation.Handling;
using LinqImplementation.Method;
using LinqImplementation.Model;
using LinqImplementation.Repositories.Data;
using LinqImplementation.Repositories.Interface;
using System;
using System.Data.SqlClient;

namespace LinqImplementation
{
    public class Program
    {
        static void Main(string[] args)
        {
            #region Initialize Connstring
            string _connString = Konstanta.connString;
            #endregion

            IEmployeeRepository empRepos = new EmployeeRepository(_connString);

            int choice = 1;

            while (choice != 6)
            {
                Console.Clear();
                Console.WriteLine("Pilih menu : ");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Deactivate Employee");
                Console.WriteLine("3. Activate Employee");
                Console.WriteLine("4. Delete Employee");
                Console.WriteLine("5. View Employee");
                Console.WriteLine("6. Exit");
                Console.Write(">> ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            empRepos.AddEmployee(new Employee());
                            continue;
                        case 2:
                            empRepos.EditEmployee();
                            continue;
                        case 3:
                            empRepos.EditEmployee(false, true);
                            continue;
                        case 4:
                            empRepos.DeleteEmployee();
                            continue;
                        case 5:
                            empRepos.PrintData();
                            Console.Read();
                            continue;
                        case 6:
                            break;
                        default:
                            Console.WriteLine("Harap memasukkan angka sesuai dengan menu yang ada.");
                            Console.ReadKey();
                            continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Harap memasukkan angka!");
                    Console.ReadKey();
                    continue;
                }
            }
        }
    }
}
