namespace AcademyPlatform.Web.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;

    public class FileUploadHelper
    {
        private static readonly IEnumerable<string> AllowedImageExtensions;

        static FileUploadHelper()
        {
            AllowedImageExtensions = new[] { ".png", ".jpg", ".jpeg" };
        }

        /// <exception cref="ArgumentException">Invalid file extension.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="uploadedImage"/> is <see langword="null" />.</exception>
        /// <exception cref="HttpException">The current <see cref="T:System.Web.HttpContext" /> is null.</exception>
        /// <exception cref="NotImplementedException">Always.</exception>
        /// <exception cref="IOException">The directory specified by <paramref name="path" /> is a file.-or-The network name is not known.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
        public static string UploadImage(HttpPostedFileBase uploadedImage, string path)
        {
            if (uploadedImage == null || uploadedImage.ContentLength == 0)
            {
                throw new ArgumentNullException("uploadedImage");
            }

            string extension = Path.GetExtension(uploadedImage.FileName);
            if (extension != null)
            {
                string fileExtension = extension.ToLowerInvariant();
                if (!AllowedImageExtensions.Contains(fileExtension))
                {
                    throw new ArgumentException(string.Format("The file is not an allowed image type: {0}", extension));
                }
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
