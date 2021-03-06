﻿namespace AcademyPlatform.Web.Areas.Admin
{
    using System.Web.Mvc;

    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional , Controller = "{controller}admin"},
                namespaces:new []{"AcademyPlatform.Web.Umbraco.Areas.Admin.Controllers"}
            );
        }
    }
}