namespace AcademyPlatform.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using AcademyPlatform.Data.Migrations;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Base;
    using AcademyPlatform.Models.Courses;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class AcademyPlatformDbContext : IdentityDbContext<User>, IAcademyPlatformDbContext
    {
        public AcademyPlatformDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AcademyPlatformDbContext, Configuration>());
        }

        public IDbSet<Course> Courses { get; set; }

        public IDbSet<Lecture> Lectures { get; set; }

        public IDbSet<HomeworkAssignment> HomeworkAssignments { get; set; }

        public IDbSet<HomeworkSubmission> HomeworkSubmissions { get; set; }

        public IDbSet<LectureResource> LectureResources { get; set; }

        public static AcademyPlatformDbContext Create()
        {
            return new AcademyPlatformDbContext();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                         e =>
                             e.Entity is ILoggedEntity && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (ILoggedEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        private void ApplyDeletableEntityRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (
                var entry in
                this.ChangeTracker.Entries()
                    .Where(e => e.Entity is ISoftDeletableEntity && (e.State == EntityState.Deleted)))
            {
                var entity = (ISoftDeletableEntity)entry.Entity;

                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}