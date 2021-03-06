﻿namespace AcademyPlatform.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Exceptions;
    using AcademyPlatform.Services.Contracts;

    public class LecturesService : ILecturesService
    {
        private readonly IRepository<Lecture> _lectures;
        private readonly IRepository<LectureVisit> _lectureVisits;
        private readonly IUserService _users;

        public LecturesService(IRepository<Lecture> lectures, IRepository<LectureVisit> lectureVisits, IUserService users)
        {
            _lectures = lectures;
            _lectureVisits = lectureVisits;
            _users = users;
        }

        public Lecture GetLectureByExternalId(int externalId)
        {
            if (externalId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(externalId), "Parameter must be a possitive integer");
            }

            return _lectures.All().FirstOrDefault(x => x.ExternalId == externalId);
        }

        public void AddOrUpdate(Lecture lecture)
        {
            var existingLecture = _lectures.All().FirstOrDefault(x => x.ExternalId == lecture.ExternalId);
            if (existingLecture == null)
            {
                _lectures.Add(lecture);
            }
            else
            {
                existingLecture.IsActive = lecture.IsActive;
                existingLecture.Title = lecture.Title;
                existingLecture.CourseId = lecture.CourseId;

                _lectures.Update(existingLecture);
            }

            _lectures.SaveChanges();
        }

        public int GetLecturesCount(int courseId)
        {
            int lecturesCount = _lectures.All().Count(x => x.CourseId == courseId && x.IsActive);
            return lecturesCount;
        }

        public int GetLectureVisitsCount(string username, int courseId)
        {
            int lectureVisitsCount =
                _lectureVisits.All().Count(x => x.User.Username == username && x.Lecture.CourseId == courseId && x.Lecture.IsActive);

            return lectureVisitsCount;
        }

        public IEnumerable<LectureVisit> GetAllLectureVisits(string username, int courseId)
        {
            List<LectureVisit> lectureVisits =
                _lectureVisits.All().Where(x => x.User.Username == username && x.Lecture.CourseId == courseId && x.Lecture.IsActive).ToList();

            return lectureVisits;
        }

        public bool HasVisitedAllLectures(string username, int courseId, out List<int> unvisitedLectureIds)
        {
            var user = _users.GetByUsername(username);
            if (user != null)
            {
                List<int> lectures = _lectures.All().Where(x => x.CourseId == courseId && x.IsActive).Select(x => x.Id).ToList();
                List<LectureVisit> lectureVisits =
                    _lectureVisits.All().Where(x => x.UserId == user.Id && lectures.Contains(x.LectureId)).ToList();
                unvisitedLectureIds = lectures.Except(lectureVisits.Select(x => x.LectureId)).ToList();
                return unvisitedLectureIds.Count == 0;
            }

            unvisitedLectureIds = null;
            return false;
        }

        public bool IsLectureVisited(string username, int externalLectureId)
        {
            User user = _users.GetByUsername(username);
            return user != null && _lectureVisits.All().Any(x => x.ExternalLectureId == externalLectureId && x.UserId == user.Id);
        }

        public void TrackLectureVisit(string username, int externalLectureId)
        {
            User user = _users.GetByUsername(username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            var lecture = _lectures.All().FirstOrDefault(x => x.ExternalId == externalLectureId);
            Debug.Assert(lecture != null);
            LectureVisit lectureVisit = _lectureVisits.All().FirstOrDefault(x => x.ExternalLectureId == externalLectureId && x.UserId == user.Id);
            if (lectureVisit == null)
            {
                _lectureVisits.Add(new LectureVisit { UserId = user.Id, ExternalLectureId = externalLectureId, LastVisitDate = DateTime.Now, LectureId = lecture.Id });
            }
            else
            {
                lectureVisit.LastVisitDate = DateTime.Now;
            }

            //TODO implement proper Unit of Work 
            _lectureVisits.SaveChanges();
        }
    }
}
