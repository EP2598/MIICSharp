using LinqImplementation.Model;
using LinqImplementation.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LinqImplementation.Repositories.Data
{
    public class RoleRepository : IRoleRepository
    {
        #region Initialize Connstring
        SqlConnection conn;

        string connString = "";
        #endregion

        public RoleRepository(string _connString)
        {
            this.connString = _connString;
        }
        public List<Roles> ViewRoles()
        {
            #region Initialize Connstring
            SqlConnection conn;

            string connString = "Data Source=LAPTOP-L3R41S4O;Initial Catalog=MCC66;User ID=sa;Password=password;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            #endregion

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
