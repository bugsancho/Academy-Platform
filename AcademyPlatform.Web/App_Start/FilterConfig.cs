﻿using AcademyPlatform.Web.Infrastructure.Filters;
using System.Web;
using System.Web.Mvc;

namespace AcademyPlatform.Web
{
    public class FilterConfig
    {

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

#if RELEASE //TODO Remove before launch
                        filters.Add(new CustomAuthorizeAttribute());
#endif
        }
    }
}
