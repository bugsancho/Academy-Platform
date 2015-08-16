namespace AcademyPlatform.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using AcademyPlatform.Models.Courses;

    internal sealed class Configuration : DbMigrationsConfiguration<AcademyPlatform.Data.AcademyPlatformDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "AcademyPlatform.Data.AcademyPlatformDbContext";
        }

        protected override void Seed(AcademyPlatform.Data.AcademyPlatformDbContext context)
        {
            //foreach (var course in context.Courses)
            //{
            //    course.CategoryId = 1;
            //}

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }

    }
}
