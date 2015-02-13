using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace QueryInterceptorAsyncCmdTest
{
    public static class SqlServerHelper
    {
        public static List<string> EnumerateServers()
        {
            var instances = SqlDataSourceEnumerator.Instance.GetDataSources();
            if (instances.Rows.Count < 1)
                return null;

            var result = new List<string>();
            foreach (DataRow instance in instances.Rows)
            {
                var serverName = instance["ServerName"].ToString();
                var instanceName = instance["InstanceName"].ToString();
                result.Add(String.IsNullOrEmpty(instanceName) ? serverName : string.Format(@"{0}\{1}", serverName, instanceName));
            }
            return result;
        }

        public static List<string> EnumerateDatabases(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var databases = connection.GetSchema("Databases");
                    connection.Close();
                    if ((databases == null) || (databases.Rows.Count < 1)) return null;

                    var result = new List<string>();
                    foreach (DataRow database in databases.Rows)
                    {
                        result.Add(database["database_name"].ToString());
                    }
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}