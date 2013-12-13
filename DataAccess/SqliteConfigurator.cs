using System.Data.SQLite;
using System.IO;
using NHibernate;
using NHibernate.Cfg;

namespace DataAccess
{
    public static class SqliteConfigurator
    {
        private static Configuration _configuration;
        private static ISessionFactory _sesionFactory;
        
        public static void Configure(string databasePath)
        {
            var connectionString = CreateConnectionString(databasePath);

            _configuration = new Configuration();
            _configuration.Configure();
            _configuration.SetProperty("connection.connection_string", connectionString);
            _configuration.AddAssembly(typeof(DataAccess.Entities.WeatherInfo).Assembly);

            _sesionFactory = _configuration.BuildSessionFactory();

            if (!File.Exists(databasePath))
            {
                CreateDatabase(databasePath);
            }
        }

        public static ISessionFactory GetSessionFactory()
        {
            return _sesionFactory;
        }

        private static string CreateConnectionString(string databasePath)
        {
            var builder = new SQLiteConnectionStringBuilder { DataSource = databasePath, Version = 3 };

            return builder.ConnectionString;
        }

        private static void BuildSchema(Configuration config, SQLiteConnection connection)
        {
            NHibernate.Tool.hbm2ddl.SchemaExport se;
            se = new NHibernate.Tool.hbm2ddl.SchemaExport(config);
            se.Execute(false, true, false, connection, null);
        }

        public static void CreateDatabase(string databasePath)
        {
            if (_configuration == null)
            {
                Configure(databasePath);
            }

            var connectionString = CreateConnectionString(databasePath);
            
            SQLiteConnection.CreateFile(databasePath);

            var connection = new SQLiteConnection(connectionString);
            connection.Open();

            BuildSchema(_configuration, connection);
        }
    }
}