namespace AcademyPlatform.Services
{
    using System;
    using System.Collections.Generic;
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
        private readonly ICertificateGenerationInfoProvider _generationInfoProvider;
        private readonly IRandomProvider _random;

        private const string CertificateUrlFormat = "https:\\focus-academy.bg\\certificate\\{0}";
        private const string CertificateFilePathFormat = "certificates\\{0}.jpeg";

        public CertificatesService(IRepository<Certificate> certificates, IRandomProvider random, IUserService users, ICoursesService courses, ICertificateGenerationInfoProvider generationInfoProvider)
        {
            _certificates = certificates;
            _random = random;
            _users = users;
            _courses = courses;
            _generationInfoProvider = generationInfoProvider;
        }

        public Certificate GetByUniqueCode(string uniqueCode)
        {
            Certificate certificate = _certificates.All().FirstOrDefault(x => x.Code == uniqueCode);
            return certificate;
        }

        public IEnumerable<Certificate> GetCertificatesForUser(string username)
        {
            List<Certificate> certificates = _certificates.AllIncluding(x => x.Course).Where(x => x.User.Username == username).ToList();
            return certificates;
        }

        public Certificate GenerateCertificate(string username, int courseId, AssessmentSubmission assessmentSubmission)
        {
            User user = _users.GetByUsername(username);
            Course course = _courses.GetById(courseId);
            CertificateGenerationInfo certificateGenerationInfo = _generationInfoProvider.GetByCourseId(courseId);
            Certificate certificate = new Certificate
            {
                UserId = user.Id,
                AssesmentSubmissionId = assessmentSubmission.Id,
                CourseId = course.Id,
                Code = _random.GenerateRandomCode(10)
            };

            Bitmap bitmap = (Bitmap)Image.FromFile(Path.GetFullPath(certificateGenerationInfo.TemplateFilePath));//load the image file

            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap certificateUrlQrCode = encoder.Encode(string.Format(CertificateUrlFormat, certificate.Code));

            using (Graphics certificateTemplate = Graphics.FromImage(bitmap))
            {
                using (Font arialFont = new Font("Arial", 16, FontStyle.Bold))
                {
                    PlaceholderInfo studentData = certificateGenerationInfo.StudentName;
                    PlaceholderInfo courseData = certificateGenerationInfo.CourseName;
                    PlaceholderInfo datePlaceholder = certificateGenerationInfo.IssueDate;
                    PlaceholderInfo qrPlaceholder = certificateGenerationInfo.QrCode;
                    certificateTemplate.DrawString($"{user.FirstName} {user.MiddleName} {user.LastName}", arialFont, new SolidBrush(ColorTranslator.FromHtml(studentData.Color)), new Rectangle(studentData.TopLeftX, studentData.TopLeftY, studentData.Width, studentData.Height));
                    certificateTemplate.DrawString(course.Title, arialFont, new SolidBrush(ColorTranslator.FromHtml(courseData.Color)), new Rectangle(courseData.TopLeftX, courseData.TopLeftY, courseData.Width, courseData.Height));
                    certificateTemplate.DrawString(DateTime.Today.ToString("dd.MM.yyy") + "г.", arialFont, new SolidBrush(ColorTranslator.FromHtml(datePlaceholder.Color)), new Rectangle(datePlaceholder.TopLeftX, datePlaceholder.TopLeftY, datePlaceholder.Width, datePlaceholder.Height));
                    certificateTemplate.DrawImage(certificateUrlQrCode, new Rectangle(qrPlaceholder.TopLeftX, qrPlaceholder.TopLeftY, qrPlaceholder.Width, qrPlaceholder.Height));

                    string certificateRelativePath = string.Format(CertificateFilePathFormat, certificate.Code);
                    string filePath = Path.Combine(certificateGenerationInfo.BaseFilePath, certificateRelativePath);
                    string directoryPath = Path.GetDirectoryName(filePath);
                    Debug.Assert(!string.IsNullOrEmpty(directoryPath));
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    bitmap.Save(filePath);
                    certificate.FilePath = certificateRelativePath;
                }
            }

            _certificates.Add(certificate);
            _certificates.SaveChanges();
            return certificate;
        }
    }
}
