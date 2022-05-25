using LinqImplementation.Model;
using LinqImplementation.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LinqImplementation.Repositories.Data
{
    public class OfficeRepository : IOfficeRepository
    {
        SqlConnection conn = new SqlConnection();
        string connString = "";

        public OfficeRepository(string _connString)
        {
            this.connString = _connString;
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
    }
}
