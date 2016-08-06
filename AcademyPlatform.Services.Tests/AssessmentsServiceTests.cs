namespace AcademyPlatform.Services.Tests
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using AcademyPlatform.Models;
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

        [Test]
        public void CreateAssesmentRequest_ShouldSaveChangesToAssesmentRepository_AfterAssmentRequestIsAdded()
        {
            var assessmentRequestRepositoryMock = new Mock<IRepository<AssessmentRequest>>();
            assessmentRequestRepositoryMock.Setup(x => x.Add(It.IsAny<AssessmentRequest>()));
            assessmentRequestRepositoryMock.Setup(x => x.SaveChanges());

            var service = SetupAssessmentsService(assessmentRequestRepositoryMock.Object);

            var assementRequest = new AssessmentRequest();

            // Act
            service.CreateAssesmentRequest(assementRequest);


            // Assert
            assessmentRequestRepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void GetAssessmentRequest_ShouldReturnAssesmentRequest_WhenGivenValidId()
        {
            var assesmentIdToGet = 5;

            var assesmentRequest = new AssessmentRequest();
            assesmentRequest.Id = assesmentIdToGet;

            var assessmentRequestRepositoryMock = new Mock<IRepository<AssessmentRequest>>();
            assessmentRequestRepositoryMock.Setup(x => x.Add(It.IsAny<AssessmentRequest>()));
            assessmentRequestRepositoryMock.Setup(x => x.SaveChanges());
            assessmentRequestRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()));

            var service = SetupAssessmentsService(assessmentRequests: assessmentRequestRepositoryMock.Object);


            service.GetAssessmentRequest(assesmentIdToGet);

            assessmentRequestRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetAssessmentRequest_ShouldThrowArgumentOutOfRangeException_WhenGivenNegativeIntegerAsId()
        {
            var assesmentIdToGet = -5;

            var assesmentRequest = new AssessmentRequest();
            assesmentRequest.Id = assesmentIdToGet;

            var assessmentRequestRepositoryMock = new Mock<IRepository<AssessmentRequest>>();
            assessmentRequestRepositoryMock.Setup(x => x.Add(It.IsAny<AssessmentRequest>()));
            assessmentRequestRepositoryMock.Setup(x => x.SaveChanges());
            assessmentRequestRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()));

            var service = SetupAssessmentsService(assessmentRequests: assessmentRequestRepositoryMock.Object);

            Assert.Throws<ArgumentOutOfRangeException>(() => service.GetAssessmentRequest(assesmentIdToGet));
        }

        [Test]
        public void GetLatestForUser_ShouldReturnAssesmentRequest_WhenGivenExistingUsernameAndAssesmentId()
        {
            #region listOfRequests
            var list = new List<AssessmentRequest>()
            {
                new AssessmentRequest()
                {
                    AssessmentExternalId = 1,
                    CreatedOn = DateTime.Now,
                    DeletedOn = null,
                    Id = 1,
                    IsCompleted = false,
                    IsDeleted = false,
                    ModifiedOn = null,
                    QuestionIds = "question ids",
                    User = new User()
                    {
                        Username = "Ivan"
                    },
                    UserId = 1
                },
                new AssessmentRequest()
                {
                    AssessmentExternalId = 2,
                    CreatedOn = DateTime.Now,
                    DeletedOn = null,
                    Id = 2,
                    IsCompleted = false,
                    IsDeleted = false,
                    ModifiedOn = null,
                    QuestionIds = "question ids",
                    User = new User()
                    {
                        Username = "Stamat"
                    },
                    UserId = 2
                },
                new AssessmentRequest()
                {
                    AssessmentExternalId = 3,
                    CreatedOn = DateTime.Now,
                    DeletedOn = null,
                    Id = 3,
                    IsCompleted = false,
                    IsDeleted = false,
                    ModifiedOn = null,
                    QuestionIds = "question ids",
                    User = new User()
                    {
                        Username = "Pesho"
                    },
                    UserId = 3
                },
            };

            #endregion

            var userPesho = new User();
            userPesho.Username = "Pesho";

            var expectedRequest = new AssessmentRequest();
            expectedRequest.User = userPesho;
            expectedRequest.AssessmentExternalId = 3;

            var assessmentRequestRepositoryMock = new Mock<IRepository<AssessmentRequest>>();
            assessmentRequestRepositoryMock.Setup(x => x.All()).Returns(list.AsQueryable());//

            var sevice = SetupAssessmentsService(assessmentRequestRepositoryMock.Object);
            var result = sevice.GetLatestForUser("Pesho", 3);

            Assert.AreEqual(expectedRequest.AssessmentExternalId, result.AssessmentExternalId);
            Assert.AreEqual(expectedRequest.User.Username, result.User.Username);
        }

        [Test]
        public void GetNextAssessmentAttemptDate_ShouldReturnNull_IfAssesmentSubmissionIsNotFound()
        {
            var service = SetupAssessmentsService();

            Assert.AreEqual(null, service.GetNextAssessmentAttemptDate("NotExistingUser", 1));
        }

       
    }
}
