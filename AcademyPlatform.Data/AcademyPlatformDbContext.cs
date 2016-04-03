namespace AcademyPlatform.Data
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Linq;

    using AcademyPlatform.Data.Migrations;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Models.Base;
    using AcademyPlatform.Models.Certificates;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Emails;
    using AcademyPlatform.Models.Payments;

    public class AcademyPlatformDbContext : DbContext, IAcademyPlatformDbContext
    {
        public AcademyPlatformDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AcademyPlatformDbContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseSubscription>()
                .Property(x => x.CourseId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CourseIdUserId", 1) { IsUnique = true }));

            modelBuilder.Entity<CourseSubscription>()
                .Property(x => x.UserId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CourseIdUserId", 2) { IsUnique = true }));

            modelBuilder.Entity<Lecture>()
                .Property(x => x.ExternalId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_LectureExternalId") { IsUnique = true }));

            modelBuilder.Entity<Lecture>()
                .Property(x => x.CourseId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_LectureCourseId")));

            modelBuilder.Entity<AssessmentSubmission>()
                        .HasRequired(x => x.Course)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<AssessmentSubmission>()
                        .HasRequired(x => x.User)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Certificate>()
                        .HasRequired(x => x.User)
                        .WithMany()
                        .WillCascadeOnDelete(false);



            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<Course> Courses { get; set; }

        public IDbSet<User> Users { get; set; }

        public IDbSet<Category> Categories { get; set; }

        public IDbSet<Lecture> Lectures { get; set; }

        public IDbSet<LectureVisit> LectureVisits { get; set; }

        public IDbSet<Certificate> Certificates { get; set; }

        public IDbSet<AssessmentRequest> AssessmentRequests { get; set; }

        public IDbSet<AssessmentSubmission> AssessmentSubmissions { get; set; }

        public IDbSet<CourseSubscription> CourseSubscriptions { get; set; }

        public IDbSet<Payment> Payments { get; set; }

        public IDbSet<Inquiry> Inquiries { get; set; }

        public IDbSet<Feedback> Feedback { get; set; }

        public static AcademyPlatformDbContext Create()
        {
            return new AcademyPlatformDbContext();
        }

        public override int SaveChanges()
        {
            ApplyAuditInfoRules();
            ApplyDeletableEntityRules();

            return base.SaveChanges();
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