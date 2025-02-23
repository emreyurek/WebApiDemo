using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AccountOwnerServer.ContextFactory
{
    public class DbContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        //veritabanı işlemlerimizi Class Library olarak ayrı bir katmanda(Repositories)
        //gerçekleştirmek istedigimiz icin bu yapiyi kullandik. Migrationlari bu projede yapacagiz
        public RepositoryContext CreateDbContext(string[] args)
        {
            // configurationBuilder -> connection stringi almak icin kullandik
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // DbContextOptionBuilder
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                prj => prj.MigrationsAssembly("AccountOwnerServer")); //migrationlarin hangi projede yurutulecegini belirttik burada

            return new RepositoryContext(builder.Options);
        }
    }
}
