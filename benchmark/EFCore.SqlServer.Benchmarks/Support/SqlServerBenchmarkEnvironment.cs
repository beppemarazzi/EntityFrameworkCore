// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.Extensions.Configuration;

#if RELEASE20 || RELEASE21 || RELEASE22 || DEBUG20 || DEBUG21 || DEBUG22
using System.Data.SqlClient;
#else
using Microsoft.Data.SqlClient;
#endif

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore.Benchmarks
{
    public static class SqlServerBenchmarkEnvironment
    {
        public static IConfiguration Config { get; }

        static SqlServerBenchmarkEnvironment()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: true)
                .AddEnvironmentVariables();

            Config = configBuilder.Build()
                .GetSection("Test:SqlServer");
        }

        private const string DefaultConnectionString
            = "Data Source=(localdb)\\MSSQLLocalDB;Database=master;Integrated Security=True;Connect Timeout=30;ConnectRetryCount=0";

        public static string DefaultConnection => Config["DefaultConnection"] ?? DefaultConnectionString;

        public static string CreateConnectionString(string name, string fileName = null, bool? multipleActiveResultSets = null)
            => new SqlConnectionStringBuilder(DefaultConnection) { InitialCatalog = name }.ToString();
    }
}
