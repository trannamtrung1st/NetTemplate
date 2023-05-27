﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NetTemplate.Common.Mediator;
using NetTemplate.Shared.ApplicationCore.Identity.Implementations;

namespace NetTemplate.Blog.Infrastructure.Persistence
{
    public class MainDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        public MainDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>()
                .UseSqlServer("Server=localhost,1434;Database=NetTemplateBlog;Trusted_Connection=False;User Id=sa;Password=z@123456!;MultipleActiveResultSets=true;TrustServerCertificate=true");

            return new MainDbContext(
                optionsBuilder.Options,
                new NullMediator(),
                new NullCurrentUserProvider());
        }
    }
}
