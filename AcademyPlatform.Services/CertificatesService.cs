namespace AcademyPlatform.Services
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    using AcademyPlatform.Common.Providers;
    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Models.Certificates;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;

    using MessagingToolkit.QRCode.Codec;

    public class CertificatesService : ICertificatesService
    {
        private readonly IRepository<Certificate> _certificates;
        private readonly IUserService _users;
        private readonly ICoursesService _courses;
        private readonly IRandomProvider _random;

        private const string CertificateUrlFormat = "https:\\focus-academy.bg\\certificate\\{0}";
        private const string CertificateFilePathFormat = "certificates\\{0}.jpeg";

        public CertificatesService(IRepository<Certificate> certificates, IRandomProvider random, IUserService users, ICoursesService courses)
        {
            _certificates = certificates;
            _random = random;
            _users = users;
            _courses = courses;
        }

        public Certificate GetByUniqueCode(string uniqueCode)
        {
            var certificate =_certificates.All().FirstOrDefault(x => x.UniqueCode == uniqueCode);
            return certificate;
        }

        public Certificate GenerateCertificate(string username, int courseId, AssessmentSubmission assessmentSubmission, CertificateGenerationInfo certificateGenerationInfo)
        {
            var user = _users.GetByUsername(username);
            var course = _courses.GetById(courseId);

            var certificate = new Certificate
            {
                //UserId = user.Id,
                AssesmentSubmissionId = assessmentSubmission.Id,
                CourseId = course.Id,
                UniqueCode = _random.GenerateRandomCode(10)
            };

            Bitmap bitmap = (Bitmap)Image.FromFile(Path.GetFullPath(certificateGenerationInfo.TemplateFilePath));//load the image file

            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap certificateUrlQrCode = encoder.Encode(string.Format(CertificateUrlFormat, certificate.UniqueCode));

            using (Graphics certificateTemplate = Graphics.FromImage(bitmap))
            {
                using (Font arialFont = new Font("Arial", 16, FontStyle.Bold))
                {
                    var studentData = certificateGenerationInfo.StudentName;
                    var courseData = certificateGenerationInfo.CourseName;
                    var datePlaceholder = certificateGenerationInfo.IssueDate;
                    var qrPlaceholder = certificateGenerationInfo.QrCode;
                    //TODO replace with middle name
                    certificateTemplate.DrawString($"{user.FirstName} {user.LastName} {user.LastName}", arialFont, new SolidBrush(ColorTranslator.FromHtml(studentData.Color)), new Rectangle(studentData.TopLeftX, studentData.TopLeftY, studentData.Width, studentData.Height));
                    certificateTemplate.DrawString(course.Title, arialFont, new SolidBrush(ColorTranslator.FromHtml(courseData.Color)), new Rectangle(courseData.TopLeftX, courseData.TopLeftY, courseData.Width, courseData.Height));
                    certificateTemplate.DrawString(DateTime.Today.ToString("dd.MM.yyy") + "г.", arialFont, new SolidBrush(ColorTranslator.FromHtml(datePlaceholder.Color)), new Rectangle(datePlaceholder.TopLeftX, datePlaceholder.TopLeftY, datePlaceholder.Width, datePlaceholder.Height));
                    certificateTemplate.DrawImage(certificateUrlQrCode, new Rectangle(qrPlaceholder.TopLeftX, qrPlaceholder.TopLeftY, qrPlaceholder.Width, qrPlaceholder.Height));
                    var filePath = Path.Combine(
                        certificateGenerationInfo.BaseFilePath,
                        string.Format(CertificateFilePathFormat, certificate.UniqueCode));
                    var directoryPath = Path.GetDirectoryName(filePath);
                    Debug.Assert(!string.IsNullOrEmpty(directoryPath));
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    bitmap.Save(filePath);
                }
            }

            
            _certificates.Add(certificate);
            _certificates.SaveChanges();
            return certificate;
        }
    }
}
