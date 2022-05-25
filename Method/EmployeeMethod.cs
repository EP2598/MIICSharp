using LinqImplementation.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LinqImplementation.Method
{
    public class EmployeeMethod
    {
        #region Initialize Connstring
        SqlConnection conn;

        string connString = "Data Source=LAPTOP-L3R41S4O;Initial Catalog=MCC66;User ID=sa;Password=password;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        #endregion

        //Iseng tambah comment
        //Comment untuk buat branch baru
        public void AddEmployee(Employee emp, bool IsError = false, int step = 1)
        {
            conn = new SqlConnection(connString);

            Console.Clear();
            if (!IsError && step == 1)
            {
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
                IsError = false;
                step = 2;
            }
            catch
            {
                Console.WriteLine("Harap masukkan format tanggal yang benar. (dd/MM/yyyy)");
                Console.ReadKey();
                AddEmployee(emp, true, 1);
            }

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
                IsError = false;
                step = 3;
            }
            catch
            {
                Console.WriteLine("Harap masukkan angka.");
                Console.ReadKey();
                AddEmployee(emp, true, 2);
            }

            if (!IsError && step == 3)
            {
                PrintData("Roles");
                Console.WriteLine("Pilih role employee : ");
            }
            Console.Write(">> ");
            try
            {
                emp.RoleId = Convert.ToInt64(Console.ReadLine());
                IsError = false;
                step = 4;
            }
            catch
            {
                Console.WriteLine("Harap masukkan angka.");
                Console.ReadKey();
                AddEmployee(emp, true, 3);
            }

            if (!IsError && step == 4)
            {
                PrintData("Office");
                Console.WriteLine("Pilih office employee : ");
            }
            Console.Write(">> ");
            try
            {
                emp.RefOfficeId = Convert.ToInt64(Console.ReadLine());
                IsError = false;
            }
            catch
            {
                Console.WriteLine("Harap masukkan angka.");
                Console.ReadKey();
                AddEmployee(emp, true, 4);
            }

            emp.IsActive = '1';

            try
            {
                conn.Open();

                SqlCommand sc = new SqlCommand("INSERT INTO EMPLOYEE(EMP_NAME, ADDRESS, BIRTHDATE, START_DATE, DIVISION_ID, ROLE_ID, REF_OFFICE_ID, ISACTIVE)" +
                    " VALUES(@EmpName, @Address, @BirthDate, @StartDate, @DivisionId, @RoleId, @RefOfficeId, @IsActive)",conn);

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

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
                Console.Read();
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
                    foreach (var item in ViewEmployee(false,0,true))
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
                    foreach (var item in ViewRegion())
                    {
                        Console.WriteLine("Region ID : " + item.RefRegionId);
                        Console.WriteLine("Region Name : " + item.RegionName);
                        Console.WriteLine("Is Active : " + item.IsActive);
                        Console.WriteLine("==================================");
                    }
                    break;
                case "Office":
                    foreach (var item in ViewOffice())
                    {
                        Console.WriteLine("Office ID : " + item.RefOfficeId);
                        Console.WriteLine("Office Name : " + item.OfficeName);
                        Console.WriteLine("Address : " + item.Address);
                        Console.WriteLine("Is Active : " + item.IsActive);
                        Console.WriteLine("==================================");
                    }
                    break;
                case "Divisions":
                    foreach (var item in ViewDivisions())
                    {
                        Console.WriteLine("Division ID : " + item.DivisionId);
                        Console.WriteLine("Division Name : " + item.DivisionName);
                        Console.WriteLine("Is Active : " + item.IsActive);
                        Console.WriteLine("==================================");
                    }
                    break;
                case "Roles":
                    foreach (var item in ViewRoles())
                    {
                        Console.WriteLine("Role ID : " + item.RoleId);
                        Console.WriteLine("Role Name : " + item.RoleName);
                        Console.WriteLine("Is Active : " + item.IsActive);
                        Console.WriteLine("==================================");
                    }
                    break;
            }
        }

        public List<Employee> ViewEmployee(bool IsRegionalView = false, long RegionId = 0, bool IsActivation = false)
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

        public List<RefRegion> ViewRegion()
        {
            List<RefRegion> refRegions = new List<RefRegion>();
            conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                SqlCommand sc = new SqlCommand("SELECT REF_REGION_ID, REGION_NAME, ISACTIVE FROM REF_REGION", conn);
                SqlDataReader reader = sc.ExecuteReader();

                while (reader.Read())
                {
                    RefRegion refRegion = new RefRegion
                    {
                        RefRegionId = Convert.ToInt64(reader["REF_REGION_ID"].ToString()),
                        RegionName = reader["REGION_NAME"].ToString(),
                        IsActive = Convert.ToChar(reader["ISACTIVE"].ToString())
                    };

                    refRegions.Add(refRegion);
                }

                reader.Close();

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
                Console.Read();
            }

            return refRegions;
        }

        public List<RefOffice> ViewOffice()
        {
            List<RefOffice> refOffices = new List<RefOffice>();
            conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                SqlCommand sc = new SqlCommand("SELECT REF_OFFICE_ID, OFFICE_NAME, ADDRESS, ISACTIVE FROM REF_OFFICE", conn);
                SqlDataReader reader = sc.ExecuteReader();

                while (reader.Read())
                {
                    RefOffice refOffice = new RefOffice
                    {
                        RefOfficeId = Convert.ToInt64(reader["REF_OFFICE_ID"].ToString()),
                        OfficeName = reader["OFFICE_NAME"].ToString(),
                        Address = reader["ADDRESS"].ToString(),
                        IsActive = Convert.ToChar(reader["ISACTIVE"].ToString())
                    };

                    refOffices.Add(refOffice);
                }

                reader.Close();

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
                Console.Read();
            }

            return refOffices;
        }

        public List<Divisions> ViewDivisions()
        {
            List<Divisions> divisions = new List<Divisions>();
            conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                SqlCommand sc = new SqlCommand("SELECT DIVISION_ID, DIVISION_NAME, ISACTIVE FROM DIVISIONS", conn);
                SqlDataReader reader = sc.ExecuteReader();

                while (reader.Read())
                {
                    Divisions division = new Divisions
                    {
                        DivisionId = Convert.ToInt64(reader["DIVISION_ID"].ToString()),
                        DivisionName = reader["DIVISION_NAME"].ToString(),
                        IsActive = Convert.ToChar(reader["ISACTIVE"].ToString())
                    };

                    divisions.Add(division);
                }

                reader.Close();

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
                Console.Read();
            }

            return divisions;
        }

        public List<Roles> ViewRoles()
        {
            List<Roles> roles = new List<Roles>();
            conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                SqlCommand sc = new SqlCommand("SELECT ROLE_ID, ROLE_NAME, ISACTIVE FROM ROLES", conn);
                SqlDataReader reader = sc.ExecuteReader();

                while (reader.Read())
                {
                    Roles role = new Roles
                    {
                        RoleId = Convert.ToInt64(reader["ROLE_ID"].ToString()),
                        RoleName = reader["ROLE_NAME"].ToString(),
                        IsActive = Convert.ToChar(reader["ISACTIVE"].ToString())
                    };

                    roles.Add(role);
                }

                reader.Close();

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
                Console.Read();
            }

            return roles;
        }
    }
}
