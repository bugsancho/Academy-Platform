namespace AcademyPlatform.Services.Contracts
{
    using System.Collections.Generic;

    using AcademyPlatform.Models.Courses;

    public interface ILecturesService
    {
        void AddOrUpdate(Lecture lecture);
        
        bool HasVisitedAllLectures(string username, int courseId, out List<int> unvisitedLectureIds);

        Lecture GetLectureByExternalId(int externalId);

        bool IsLectureVisited(string username, int externalLectureId);

        void TrackLectureVisit(string username, int externalLectureId);
    }
}