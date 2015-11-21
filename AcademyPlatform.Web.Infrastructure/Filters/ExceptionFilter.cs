namespace AcademyPlatform.Web.Infrastructure.Filters
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using log4net;

    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
	        if (context.ExceptionHandled) return;

            Exception ex = context.Exception;
            // will not log "File Not Found" errors
            if (!(ex is HttpException)) 
            {
                ILog log = LogManager.GetLogger(context.Controller.GetType());
                if (log.IsErrorEnabled)
                {
                    log.Error(ex.Message, ex);
                }
            }
        }
    }
}
