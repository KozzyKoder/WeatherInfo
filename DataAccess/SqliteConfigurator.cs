﻿using System.Data.SQLite;
using System.IO;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

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
            _configuration.AddAssembly(typeof(Entities.WeatherInfo).Assembly);

            if (!File.Exists(databasePath))
            {
                CreateDatabase(databasePath);
            }

            _sesionFactory = _configuration.BuildSessionFactory();
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
            var se = new SchemaExport(config);
            se.Execute(false, true, false, connection, null);
        }

        private static void CreateDatabase(string databasePath)
        {
            if (_configuration == null)
            {
                Configure(databasePath);
            }

            var connectionString = CreateConnectionString(databasePath);

            var fullPathToDatabase = Path.GetDirectoryName(databasePath);
            if (fullPathToDatabase != null && !Directory.Exists(fullPathToDatabase))
            {
                Directory.CreateDirectory(fullPathToDatabase);
            }

            SQLiteConnection.CreateFile(databasePath);

            var connection = new SQLiteConnection(connectionString);
            connection.Open();

            BuildSchema(_configuration, connection);
        }
    }
}