namespace AcademyPlatform.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Payments;

    public interface IAcademyPlatformDbContext
    {
        DbSet<T> Set<T>() where T : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();

        void Dispose();

        IDbSet<CourseSubscription> CourseSubscriptions { get; set; }

        IDbSet<Payment> Payments { get; set; }
    }
}