using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NetTemplate.Common.Mediator.Implementations;
using NetTemplate.Shared.ApplicationCore.Identity.Implementations;

namespace NetTemplate.Identity.Infrastructure.Persistence
{
    public class MainDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        public MainDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>()
                .UseSqlServer("Server=localhost,1434;Database=NetTemplateIdentity;Trusted_Connection=False;User Id=sa;Password=z@123456!;MultipleActiveResultSets=true;TrustServerCertificate=true");

            return new MainDbContext(
                optionsBuilder.Options,
                new NullMediator(),
                new NullCurrentUserProvider());
        }
    }
}
