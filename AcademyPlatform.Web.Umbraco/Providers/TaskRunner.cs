namespace AcademyPlatform.Web.Umbraco.Providers
{
    using System;
    using System.Linq.Expressions;

    using AcademyPlatform.Services.Contracts;

    using Hangfire;

    public class TaskRunner : ITaskRunner
    {
        public void Run<T>(Expression<Action<T>> task)
        {
            BackgroundJob.Enqueue(task);
        }

        public void Schedule<T>(Expression<Action<T>> task, DateTimeOffset runAt)
        {
            BackgroundJob.Schedule(task, runAt);
        }

        public void ScheduleRecurring<T>(string taskName, Expression<Action<T>> task, string cronExpression)
        {
            RecurringJob.AddOrUpdate(taskName, task, cronExpression);
        }
    }
}