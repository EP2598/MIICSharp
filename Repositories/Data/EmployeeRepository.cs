using LinqImplementation.Model;
using LinqImplementation.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LinqImplementation.Repositories.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        SqlConnection conn = new SqlConnection();
        string connString = "";

        public EmployeeRepository(string _connString)
        {
            this.connString = _connString;
        }

        public void AddEmployee(Employee emp, bool IsError = false, int step = 1)
        {

            #region Input Name, Address, Birthdate
            if (!IsError && step == 1)
            {
                Console.Clear();
                emp = new Employee();

                Console.WriteLine("Input nama employee : ");
                Console.Write(">> ");
                emp.EmpName = Console.ReadLine();
                Console.WriteLine("Input alamat employee : ");
                Console.Write(">> ");
                emp.Address = Console.ReadLine();
                Console.WriteLine("Input tanggal lahir employee : ");
            }
            Console.Write(">> ");
            try
            {
                emp.BirthDate = Convert.ToDateTime(Console.ReadLine());
            }
            catch
            {
                AddEmployee(ErrorHandler<Employee>("InvalidDate", emp), true, 1);
            }
            finally
            {
                IsError = false;
                step++;
            } 
            #endregion

            #region Input Division
            if (!IsError && step == 2)
            {
                emp.StartDate = DateTime.Now;
                PrintData("Divisions");
                Console.WriteLine("Pilih divisi employee : ");
            }
            Console.Write(">> ");
            try
            {
                emp.DivisionId = Convert.ToInt64(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Harap masukkan angka.");
                Console.ReadKey();
                AddEmployee(ErrorHandler<Employee>("InputMustBeNumber", emp), true, 2);
            }
            finally
            {
                IsError = false;
                step++;
            }
            #endregion

            #region Input Roles
            if (!IsError && step == 3)
            {
                PrintData("Roles");
                Console.WriteLine("Pilih role employee : ");
            }
            Console.Write(">> ");
            try
            {
                emp.RoleId = Convert.ToInt64(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Harap masukkan angka.");
                Console.ReadKey();
                AddEmployee(ErrorHandler<Employee>("InputMustBeNumber", emp), true, 3);
            }
            finally
            {
                IsError = false;
                step++;
            }
            #endregion

            #region Input Office
            if (!IsError && step == 4)
            {
                PrintData("Office");
                Console.WriteLine("Pilih office employee : ");
            }
            Console.Write(">> ");
            try
            {
                emp.RefOfficeId = Convert.ToInt64(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Harap masukkan angka.");
                Console.ReadKey();
                AddEmployee(ErrorHandler<Employee>("InputMustBeNumber", emp), true, 4);
            }
            finally
            {
                IsError = false;
                emp.IsActive = '1';
            } 
            #endregion
              
            conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                SqlCommand sc = new SqlCommand("INSERT INTO EMPLOYEE(EMP_NAME, ADDRESS, BIRTHDATE, START_DATE, DIVISION_ID, ROLE_ID, REF_OFFICE_ID, ISACTIVE)" +
                    " VALUES(@EmpName, @Address, @BirthDate, @StartDate, @DivisionId, @RoleId, @RefOfficeId, @IsActive)", conn);

                #region Initialize Parameter
                sc.Parameters.Add(new SqlParameter("EmpName", emp.EmpName));
                sc.Parameters.Add(new SqlParameter("Address", emp.Address));
                sc.Parameters.Add(new SqlParameter("BirthDate", emp.BirthDate));
                sc.Parameters.Add(new SqlParameter("StartDate", emp.StartDate));
                sc.Parameters.Add(new SqlParameter("DivisionId", emp.DivisionId));
                sc.Parameters.Add(new SqlParameter("RoleId", emp.RoleId));
                sc.Parameters.Add(new SqlParameter("RefOfficeId", emp.RefOfficeId));
                sc.Parameters.Add(new SqlParameter("IsActive", emp.IsActive));
                #endregion

                sc.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
                Console.Read();
            }
            finally
            {
                conn.Close();
            }

            Console.WriteLine("Success add data");
        }

        public void EditEmployee(bool IsError = false, bool IsActivation = false)
        {
            conn = new SqlConnection(connString);

            long EmpId = 0;
            char IsActive = '0';

            if (!IsError)
            {
                if (IsActivation)
                {
                    PrintData("Activation");
                    Console.WriteLine("Pilih ID employee yang akan diaktifkan: ");
                    IsActive = '1';
                }
                else
                {
                    PrintData();
                    Console.WriteLine("Pilih ID employee yang akan dinonaktifkan: ");
                }

            }
            Console.Write(">> ");

            try
            {
                EmpId = Convert.ToInt64(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Harap masukkan angka.");
                Console.ReadKey();
                EditEmployee(true);
            }

            try
            {
                conn.Open();

                SqlCommand sc = new SqlCommand("UPDATE EMPLOYEE SET ISACTIVE = @IsActive " +
                    "WHERE EMP_ID = @EmpId", conn);

                #region Initialize Parameter
                sc.Parameters.Add(new SqlParameter("EmpId", EmpId));
                sc.Parameters.Add(new SqlParameter("IsActive", IsActive));
                #endregion

                sc.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
                Console.Read();
            }

            Console.WriteLine("Success edit data");
        }

        public void DeleteEmployee(bool IsError = false)
        {
            conn = new SqlConnection(connString);

            long EmpId = 0;

            if (!IsError)
            {
                PrintData();

                Console.WriteLine("Pilih ID employee yang akan dihapus: ");
            }
            Console.Write(">> ");

            try
            {
                EmpId = Convert.ToInt64(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Harap masukkan angka.");
                Console.ReadKey();
                DeleteEmployee(true);
            }

            try
            {
                conn.Open();

                SqlCommand sc = new SqlCommand("DELETE FROM EMPLOYEE WHERE EMP_ID = @EmpId", conn);

                #region Initialize Parameter
                sc.Parameters.Add(new SqlParameter("EmpId", EmpId));
                #endregion

                sc.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
                Console.Read();
            }

            Console.WriteLine("Success delete data");
        }

        public void PrintData(string dataType = "Employee")
        {
            IRegionRepository regionRepos = new RegionRepository(connString);
            IOfficeRepository officeRepos = new OfficeRepository(connString);
            IDivisionRepository divisionRepos = new DivisionRepository(connString);
            IRoleRepository roleRepos = new RoleRepository(connString);

            Console.Clear();

            switch (dataType)
            {
                case "Employee":
                    foreach (var item in ViewEmployee())
                    {
                        Console.WriteLine("Employee ID : " + item.EmpId);
                        Console.WriteLine("Name : " + item.EmpName);
                        Console.WriteLine("Address : " + item.Address);
                        Console.WriteLine("Birth date : " + item.BirthDate);
                        Console.WriteLine("Start Date : " + item.StartDate);
                        Console.WriteLine("Division ID : " + item.DivisionId);
                        Console.WriteLine("Role ID : " + item.RoleId);
                        Console.WriteLine("Office ID : " + item.RefOfficeId);
                        Console.WriteLine("Is Active : " + item.IsActive);
                        Console.WriteLine("==================================");
                    }
                    break;
                case "Activation":
                    foreach (var item in ViewEmployee(false, 0, true))
                    {
                        Console.WriteLine("Employee ID : " + item.EmpId);
                        Console.WriteLine("Name : " + item.EmpName);
                        Console.WriteLine("Address : " + item.Address);
                        Console.WriteLine("Birth date : " + item.BirthDate);
                        Console.WriteLine("Start Date : " + item.StartDate);
                        Console.WriteLine("Division ID : " + item.DivisionId);
                        Console.WriteLine("Role ID : " + item.RoleId);
                        Console.WriteLine("Office ID : " + item.RefOfficeId);
                        Console.WriteLine("Is Active : " + item.IsActive);
                        Console.WriteLine("==================================");
                    }
                    break;
                case "Region":
                    foreach (var item in regionRepos.ViewRegion())
                    {
                        Console.WriteLine("Region ID : " + item.RefRegionId);
                        Console.WriteLine("Region Name : " + item.RegionName);
                        Console.WriteLine("Is Active : " + item.IsActive);
                        Console.WriteLine("==================================");
                    }
                    break;
                case "Office":
                    foreach (var item in officeRepos.ViewOffice())
                    {
                        Console.WriteLine("Office ID : " + item.RefOfficeId);
                        Console.WriteLine("Office Name : " + item.OfficeName);
                        Console.WriteLine("Address : " + item.Address);
                        Console.WriteLine("Is Active : " + item.IsActive);
                        Console.WriteLine("==================================");
                    }
                    break;
                case "Divisions":
                    foreach (var item in divisionRepos.ViewDivisions())
                    {
                        Console.WriteLine("Division ID : " + item.DivisionId);
                        Console.WriteLine("Division Name : " + item.DivisionName);
                        Console.WriteLine("Is Active : " + item.IsActive);
                        Console.WriteLine("==================================");
                    }
                    break;
                case "Roles":
                    foreach (var item in roleRepos.ViewRoles())
                    {
                        Console.WriteLine("Role ID : " + item.RoleId);
                        Console.WriteLine("Role Name : " + item.RoleName);
                        Console.WriteLine("Is Active : " + item.IsActive);
                        Console.WriteLine("==================================");
                    }
                    break;
            }
        }

        private List<Employee> ViewEmployee(bool IsRegionalView = false, long RegionId = 0, bool IsActivation = false)
        {
            List<Employee> employees = new List<Employee>();
            conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                string strQuery = "SELECT EMP.EMP_ID, EMP.EMP_NAME, EMP.ADDRESS, EMP.BIRTHDATE, EMP.START_DATE, " +
                    "EMP.DIVISION_ID, EMP.ROLE_ID, EMP.REF_OFFICE_ID, EMP.ISACTIVE FROM EMPLOYEE EMP";

                if (IsRegionalView)
                {
                    strQuery += " JOIN REF_OFFICE RO ON EMP.REF_OFFICE_ID = RO.REF_OFFICE_ID " +
                        "JOIN REF_AREA RA ON RO.REF_AREA_ID = RA.REF_AREA_ID" +
                        "JOIN REF_REGION RG ON RA.REF_REGION_ID = RG.REF_REGION_ID ";
                }

                if (IsActivation)
                {
                    strQuery += " WHERE EMP.ISACTIVE = '0'";
                }
                else
                {
                    strQuery += " WHERE EMP.ISACTIVE = '1'";
                }

                if (IsRegionalView)
                {
                    strQuery += " AND RG.REF_REGION_ID = @RegionId";
                }

                SqlCommand sc = new SqlCommand(strQuery, conn);

                if (IsRegionalView)
                {
                    sc.Parameters.Add(new SqlParameter("RegionId", RegionId));
                }

                SqlDataReader reader = sc.ExecuteReader();

                while (reader.Read())
                {
                    Employee emp = new Employee
                    {
                        EmpId = Convert.ToInt64(reader["EMP_ID"].ToString()),
                        EmpName = reader["EMP_NAME"].ToString(),
                        Address = reader["ADDRESS"].ToString(),
                        BirthDate = Convert.ToDateTime(reader["BIRTHDATE"].ToString()),
                        StartDate = Convert.ToDateTime(reader["START_DATE"].ToString()),
                        DivisionId = Convert.ToInt64(reader["DIVISION_ID"].ToString()),
                        RoleId = Convert.ToInt64(reader["ROLE_ID"].ToString()),
                        RefOfficeId = Convert.ToInt64(reader["REF_OFFICE_ID"].ToString()),
                        IsActive = Convert.ToChar(reader["ISACTIVE"].ToString())
                    };

                    employees.Add(emp);
                }


                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
                Console.Read();
            }

            return employees;
        }

        private static T ErrorHandler<T>(string errName, T Entity) where T : class 
        {
            switch (errName)
            {
                case "InputMustBeNumber":
                    Console.WriteLine("Harap masukkan angka dengan benar.");
                    break;
                case "InvalidDate":
                    Console.WriteLine("Harap masukkan tanggal dengan benar (dd/MM/yyyy)");
                    break;
                case "InvalidChoice":
                    Console.WriteLine("Harap masukkan pilihan sesuai dengan menu yang ada.");
                    break;
            }
            Console.WriteLine("Tekan enter untuk mengulang.");
            Console.ReadKey();
            return Entity;
        }
    }
}
