namespace AcademyPlatform.Web.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.AccessControl;
    using System.Web;

    public class FileUploadHelper
    {
        private static readonly IEnumerable<string> AllowedImageExtensions;

        static FileUploadHelper()
        {
            AllowedImageExtensions = new string[] { ".png", ".jpg", ".jpeg" };
        }

        public static string UploadImage(HttpPostedFileBase uploadedImage, string path)
        {
            if (uploadedImage == null || uploadedImage.ContentLength == 0)
            {
                throw new ArgumentNullException("uploadedImage");
            }

            if (!AllowedImageExtensions.Contains(Path.GetExtension(uploadedImage.FileName)))
            {
                throw new ArgumentException("The file is not an allowed image type");
            }

            string serverPath = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }

            string imagePath = Path.Combine(serverPath, uploadedImage.FileName);

            uploadedImage.SaveAs(imagePath);

            return Path.Combine(path, uploadedImage.FileName);
        }
    }
}
