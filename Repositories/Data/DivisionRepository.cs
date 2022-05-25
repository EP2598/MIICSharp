using LinqImplementation.Model;
using LinqImplementation.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LinqImplementation.Repositories.Data
{
    public class DivisionRepository : IDivisionRepository
    {
        SqlConnection conn = new SqlConnection();
        string connString = "";

        public DivisionRepository(string _connString)
        {
            this.connString = _connString;
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
    }
}
