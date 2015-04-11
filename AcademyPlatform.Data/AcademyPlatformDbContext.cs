namespace AcademyPlatform.Data
{
    using System;
    using System.Data.Entity;
    using AcademyPlatform.Data.Migrations;
    using AcademyPlatform.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class AcademyPlatformDbContext : IdentityDbContext<User>, IAcademyPlatformDbContext
    {
        public AcademyPlatformDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AcademyPlatformDbContext, Configuration>());
        }

        public IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public static AcademyPlatformDbContext Create()
        {
            return new AcademyPlatformDbContext();
        }
    }
}