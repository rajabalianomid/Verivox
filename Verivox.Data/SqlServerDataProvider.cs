using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Verivox.Common;
using Verivox.Common.Data;

namespace Verivox.Data
{
    public class SqlServerDataProvider
    {

        public static void InitializeDatabase(string conectionString)
        {
            CreateDatabase(conectionString);

            IDbContext context = EngineContext.Current.Resolve<IDbContext>();

            List<string> tableNamesToValidate = new List<string> { "ProductType", "Product" };
            List<string> existingTableNames = context
                .QueryFromSql<StringQueryType>("SELECT table_name AS Value FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE'")
                .Select(stringValue => stringValue.Value).ToList();
            bool createTables = !existingTableNames.Intersect(tableNamesToValidate, StringComparer.InvariantCultureIgnoreCase).Any();
            if (createTables)
            {

                IVerivoxFileProvider fileProvider = EngineContext.Current.Resolve<IVerivoxFileProvider>();

                //create tables
                //EngineContext.Current.Resolve<IRelationalDatabaseCreator>().CreateTables();
                //(context as DbContext).Database.EnsureCreated();
                context.ExecuteSqlScript(context.GenerateCreateScript());

                //create indexes
                context.ExecuteSeedFromJsonFile(fileProvider.MapPath(DataDefaults.JsonFilePath));
            }
        }

        private static void CreateDatabase(string connectionString, int triesToConnect = 10)
        {
            if (!SqlServerDatabaseExists(connectionString))
            {
                try
                {
                    //parse database name
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                    string databaseName = builder.InitialCatalog;
                    //now create connection string to 'master' dabatase. It always exists.
                    builder.InitialCatalog = "master";
                    string masterCatalogConnectionString = builder.ToString();
                    string query = $"CREATE DATABASE [{databaseName}]";
                    using (SqlConnection conn = new SqlConnection(masterCatalogConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand command = new SqlCommand(query, conn))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    //try connect
                    if (triesToConnect > 0)
                    {
                        //Sometimes on slow servers (hosting) there could be situations when database requires some time to be created.
                        //But we have already started creation of tables and sample data.
                        //As a result there is an exception thrown and the installation process cannot continue.
                        //That's why we are in a cycle of "triesToConnect" times trying to connect to a database with a delay of one second.
                        for (int i = 0; i <= triesToConnect; i++)
                        {
                            if (i == triesToConnect)
                            {
                                throw new Exception("Unable to connect to the new database. Please try one more time");
                            }

                            if (!SqlServerDatabaseExists(connectionString))
                            {
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        private static bool SqlServerDatabaseExists(string connectionString)
        {
            try
            {
                //just try to connect
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
