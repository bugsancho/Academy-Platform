namespace AcademyPlatform.Web.Infrastructure.Extensions
{
    public static class IntExtensions
    {
        public static string ToFileSize(this int fileSize)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (fileSize >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                fileSize = fileSize / 1024;
            }
            
            string result = $"{fileSize:0.##} {sizes[order]}";
            return result;
        }
    }
}
