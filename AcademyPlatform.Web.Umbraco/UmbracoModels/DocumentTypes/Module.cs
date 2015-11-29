namespace AcademyPlatform.Web.Umbraco.UmbracoModels.DocumentTypes
{
    using System.Collections.Generic;

    public class Module : DocumentTypeBase
    {
        public Module()
        {
            Lectures = new List<Lecture>();
        }

        public IEnumerable<Lecture> Lectures { get; set; }
    }
}