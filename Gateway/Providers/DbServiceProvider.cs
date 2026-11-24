using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using Gateway.Interfaces;

namespace Gateway.Providers
{
    public class DbServiceProvider : IDbServiceProvider
    {
        public SqlConnection GetSqlConnection(string connectionString)
        {
            //TestConnection(connectionString);
            return new SqlConnection(connectionString); 
        }

        public static bool TestConnection(string connectionString)
        {
            //errorMessage = string.Empty;

            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                return true; // Connection succeeded
            }
            catch (Exception ex)
            {
                //errorMessage = ex.Message;
                return false; // Connection failed
            }
        }
    }
}
