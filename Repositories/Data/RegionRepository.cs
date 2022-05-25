using LinqImplementation.Model;
using LinqImplementation.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LinqImplementation.Repositories.Data
{
    public class RegionRepository : IRegionRepository
    {
        #region Initialize Connstring
        SqlConnection conn;

        string connString = "";
        #endregion

        public RegionRepository(string _connString)
        {
            this.connString = _connString;
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
    }
}
