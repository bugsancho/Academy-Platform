namespace AcademyPlatform.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public interface IAcademyPlatformDbContext
    {
        IDbSet<T> Set<T>() where T : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}