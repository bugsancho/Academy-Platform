namespace AcademyPlatform.Services.Tests
{
    using System;
    using System.Collections.Generic;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Services.Tests.Mocks;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class LecturesServiceTests
    {
        [Test]
        public void GetLectureByExternalId_ReturnsLectureWithCorrectId_WhenLectureExists()
        {
            // Arrange
            List<Lecture> testLectures = new List<Lecture>
                                   {
                                       new Lecture { ExternalId = 1,Id = 3123, Title = "Lecture 1" },
                                       new Lecture { ExternalId = 2, Id = 4324,Title = "Lecture 2" }
                                   };

            IRepository<Lecture> repository = new RepositoryMock<Lecture>();
            testLectures.ForEach(lecture => repository.Add(lecture));

            ILecturesService service = SetupLecturesService(repository);

            int lectureId = 1;

            // Act
            Lecture returnedLecture = service.GetLectureByExternalId(lectureId);

            // Assert
            Assert.IsTrue(returnedLecture.ExternalId == lectureId);
        }

        [Test]
        public void GetLectureByExternalId_ReturnsNull_WhenLectureDoesntExist()
        {
            // Arrange
            List<Lecture> testLectures = new List<Lecture>
                                   {
                                       new Lecture { ExternalId = 1,Id = 3123, Title = "Lecture 1" },
                                       new Lecture { ExternalId = 2, Id = 4324,Title = "Lecture 2" }
                                   };

            IRepository<Lecture> repository = new RepositoryMock<Lecture>();
            testLectures.ForEach(lecture => repository.Add(lecture));

            ILecturesService service = SetupLecturesService(repository);

            int lectureId = 6;

            // Act
            Lecture returnedLecture = service.GetLectureByExternalId(lectureId);

            // Assert
            Assert.IsNull(returnedLecture);

        }

        [Test]
        public void GetLectureByExternalId_ThrowsArgumentOutOfRangeException_WhenLectureIdIsNegative()
        {
            // Arrange
            ILecturesService service = SetupLecturesService();

            int lectureId = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.GetLectureByExternalId(lectureId));
        }

        [Test]
        public void GetLectureByExternalId_ThrowsArgumentOutOfRangeException_WhenLectureIdIsZero()
        {
            // Arrange
            ILecturesService service = SetupLecturesService();

            int lectureId = 0;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.GetLectureByExternalId(lectureId));
        }

        private ILecturesService SetupLecturesService(IRepository<Lecture> lectures = null, IRepository<LectureVisit> lectureVisits = null, IUserService users = null)
        {
            if (lectures == null)
            {
                lectures = new Mock<IRepository<Lecture>>().Object;
            }
            if (lectureVisits == null)
            {
                lectureVisits = new Mock<IRepository<LectureVisit>>().Object;
            }
            if (users == null)
            {
                users = new Mock<IUserService>().Object;
            }

            LecturesService lecturesService = new LecturesService(lectures, lectureVisits, users);

            return lecturesService;
        }
    }
}