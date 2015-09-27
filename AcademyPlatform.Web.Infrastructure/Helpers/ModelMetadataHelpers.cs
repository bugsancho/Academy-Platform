namespace AcademyPlatform.Web.Infrastructure.Helpers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Web.Mvc;

    public static class ModelMetadataHelpers
    {
        public static IEnumerable<ModelMetadata> GetPropertiesForDisplay(ViewDataDictionary viewData)
        {
            return viewData.ModelMetadata.Properties;
        }

        public static IEnumerable<ModelMetadata> GetPropertiesForEdit(ViewDataDictionary viewData)
        {
            return viewData.ModelMetadata.Properties;
        }

        private static bool ShouldShowForEdit(ModelMetadata metadata, ViewDataDictionary viewData)
        {
            return metadata.ShowForEdit && ShouldShow(metadata, viewData);
        }

        private static bool ShouldShowForDisplay(ModelMetadata metadata, ViewDataDictionary viewData)
        {
            return metadata.ShowForDisplay && ShouldShow(metadata, viewData);
        }

        private static bool ShouldShow(ModelMetadata metadata, ViewDataDictionary viewData)
        {
            return metadata.ModelType != typeof(EntityState) &&
                   !metadata.IsComplexType &&
                   !viewData.TemplateInfo.Visited(metadata);
        }
    }
}