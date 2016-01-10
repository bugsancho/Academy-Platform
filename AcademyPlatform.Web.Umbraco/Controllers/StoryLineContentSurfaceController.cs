namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    using AcademyPlatform.Web.Models.Other;
    using AcademyPlatform.Web.Models.Umbraco.Macros;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    public class StoryLineContentSurfaceController : SurfaceController
    {
        private const string FilePathRegex = @"^\/media\/(\d{4,})\/(.+)\.zip$";

        private const string StorylineFileName = "story.html";

        public ActionResult RenderStorylineContent(int contentZipArchive)
        {
            var storylineContentNode = Umbraco.TypedContent(contentZipArchive);
            if (storylineContentNode.DocumentTypeAlias != nameof(StorylineContent))
            {
                throw new ArgumentException($"Content with id {contentZipArchive} is not a valid {nameof(StorylineContent)} node", nameof(contentZipArchive));
            }

            var archivePath = storylineContentNode.GetPropertyValue<string>(nameof(StorylineContent.ContentZipArchive));
            var contentWidth = storylineContentNode.GetPropertyValue<int>(nameof(StorylineContent.Width));
            var contentHeight = storylineContentNode.GetPropertyValue<int>(nameof(StorylineContent.Height));

            string storylineContentPath = string.Empty;

            var regexResult = Regex.Match(archivePath, FilePathRegex);
            if (regexResult.Success)
            {
                var fileId = regexResult.Groups[1].ToString();
                var fileName = regexResult.Groups[2].ToString();
                var targetDirectory = $"~/StorylineContent/{fileName}-{fileId}/";
                var serverDirectory = Server.MapPath(targetDirectory);

                if (!Directory.Exists(serverDirectory))
                {
                    Directory.CreateDirectory(serverDirectory);
                    using (FileStream archiveStream = new FileStream(Server.MapPath("~" + archivePath), FileMode.Open))
                    {
                        using (ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Read))
                        {
                            archive.ExtractToDirectory(serverDirectory);
                        }
                    }
                }

                if (System.IO.File.Exists(Path.Combine(serverDirectory, StorylineFileName)))
                {
                    storylineContentPath = Path.Combine(targetDirectory.Replace("~", ""), StorylineFileName);
                }
                else
                {
                    throw new ArgumentException($"Storyline content archive doesnt contain file with name {StorylineFileName}");
                }
            }
            else
            {
                throw new ArgumentException("The attached file is not it the proper format. Please attach a Zip archive!", nameof(contentZipArchive));
            }

            var storylineContentViewModel = new StorylineContentViewModel
            {
                ContentFilePath = storylineContentPath,
                Width = contentWidth,
                Height = contentHeight
            };

            return View("~/Views/RenderViews/RenderStorylineContent.cshtml",storylineContentViewModel);
        }
    }
}