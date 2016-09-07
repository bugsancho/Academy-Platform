namespace AcademyPlatform.Services.Contracts
{
    using System;
    using System.Linq.Expressions;

    public interface ITaskRunner
    {
        void Run<T>(Expression<Action<T>> task);

        void Schedule<T>(Expression<Action<T>> task, DateTimeOffset runAt);

        void ScheduleRecurring<T>(string taskName, Expression<Action<T>> task, string cronExpression);
    }
}
