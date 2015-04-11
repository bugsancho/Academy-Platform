namespace AcademyPlatform.Data
{
    using System;
    using System.Data.Entity;

    public class AcademyPlatformDbContext : DbContext, IAcademyPlatformDbContext
    {
        public IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
    }
}
