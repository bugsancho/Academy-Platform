namespace AcademyPlatform.Services.Tests
{
    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;

    using Data.Repositories;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AssessmentsServiceTests
    {
        private IAssessmentsService SetupAssessmentsService(
            IRepository<AssessmentRequest> assessmentRequests = null,
            IRepository<AssessmentSubmission> assessmentSubmissions = null,
            IMessageService messageService = null,
            ITaskRunner taskRunner = null,
            ILecturesService lecturesService = null,
            IApplicationSettings applicationSettings = null,
            IRepository<CourseSubscription> subscriptions = null,
            IRouteProvider routeProvider = null,
            ICertificatesService certificatesService = null,
            IRepository<Course> courses = null)
        {
            if (assessmentRequests == null)
            {
                assessmentRequests = new Mock<IRepository<AssessmentRequest>>().Object;
            }

            if (assessmentSubmissions == null)
            {
                assessmentSubmissions = new Mock<IRepository<AssessmentSubmission>>().Object;
            }

            if (messageService == null)
            {
                messageService = new Mock<IMessageService>().Object;
            }

            if (taskRunner == null)
            {
                taskRunner = new Mock<ITaskRunner>().Object;
            }

            if (lecturesService == null)
            {
                lecturesService = new Mock<ILecturesService>().Object;
            }

            if (applicationSettings == null)
            {
                applicationSettings = new Mock<IApplicationSettings>().Object;
            }

            if (subscriptions == null)
            {
                subscriptions = new Mock<IRepository<CourseSubscription>>().Object;
            }

            if (routeProvider == null)
            {
                routeProvider = new Mock<IRouteProvider>().Object;
            }

            if (certificatesService == null)
            {
                certificatesService = new Mock<ICertificatesService>().Object;
            }

            if (courses == null)
            {
                courses = new Mock<IRepository<Course>>().Object;
            }

            var service = new AssessmentsService(assessmentRequests, assessmentSubmissions, messageService, taskRunner, lecturesService, applicationSettings, subscriptions, routeProvider, certificatesService, courses);
            return service;
        }

        [Test]
        public void CreateAssesmentRequest_ShouldAddAssesmentToRepositoryOnlyOnce_WhenAssesmentRequestIsNull()
        {
            // Arrange
            var assessmentRequestRepositoryMock = new Mock<IRepository<AssessmentRequest>>();
            assessmentRequestRepositoryMock.Setup(x => x.Add(It.IsAny<AssessmentRequest>()));

            var service = SetupAssessmentsService(assessmentRequestRepositoryMock.Object);

            var assementRequest = new AssessmentRequest();

            // Act
            service.CreateAssesmentRequest(assementRequest);

            // Assert
            assessmentRequestRepositoryMock.Verify(x => x.Add(It.IsAny<AssessmentRequest>()), Times.Once);
        }
    }
}
