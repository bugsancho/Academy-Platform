namespace AcademyPlatform.Web.Umbraco.Startup
{
    using global::Umbraco.Core;

    using Newtonsoft.Json;

    public class SerializationConfig : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
    }
}