using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserManagementApp.Data.Contexts;
using UserManagementApp.Data.Interfaces;

namespace UserManagementApp.Data.Providers
{
    public static class DataProviderFactory
    {
        public static IDataProvider CreateDataProvider()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
            string providerType = configuration["DataProvider"];

            switch (providerType)
            {
                case "InMemory":
                    return new InMemoryDataProvider();

                case "Xml":
                    string xmlFilePath = configuration["XmlFilePath"];
                    return new XmlDataProvider(xmlFilePath);

                case "EfCoreMsSql":
                    string connectionString = configuration.GetConnectionString("MsSql");
                    var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
                    optionsBuilder.UseSqlServer(connectionString);
                    var dbContext = new UserDbContext(optionsBuilder.Options);
                    return new EfCoreMsSqlDataProvider(dbContext);

                default:
                    throw new Exception($"Unknown DataProvider type: {providerType}");
            }
        }
    }
}
