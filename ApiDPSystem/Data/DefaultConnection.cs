using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Data
{
    public static class DefaultConnection
    {
        public const string DefaultConnectionName = "DefaultConnection";
        public static string DefaultConnectionString
        {
            get
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json");
                var configContext = builder.Build();
                return configContext.GetConnectionString(DefaultConnectionName);
            }
        }
    }
}
