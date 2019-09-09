using Exam.Services;
using ExamContract.CourseModels;
using ExamContract.ExamDTO;
using ExamContract.TutorialModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using exam = ExamContract.MainDbModels.Exam;
using examCourse = ExamContract.CourseModels.ExamCourse;

namespace Exam.Data.UnitOfWork
{
    public class Courses
    {
        public readonly WebApiClient<Course> CourseRepo;
        public readonly WebApiClient<ExamContract.CourseModels.User> UsersRepo;
        private readonly WebApiClient<exam> ExamsRepo;
        private readonly WebApiClient<Tutorial> TutorialsRepo;
        private readonly ILogger logger;
        private readonly CourseTwoKeyApiClient<examCourse> examCourses;
        private readonly CourseTwoKeyApiClient<TutorialCourse> tutorialCourses;

       

        public Courses(WebApiClient<Course> courses, WebApiClient<Tutorial> tutorials, WebApiClient<ExamContract.CourseModels.User> users, WebApiClient<exam> exams, CourseTwoKeyApiClient<examCourse> examCourses, CourseTwoKeyApiClient<TutorialCourse> tutorialCourses, ILogger logger)
        {
            CourseRepo = courses;
            TutorialsRepo = tutorials;
            UsersRepo = users;
            ExamsRepo = exams;
            this.examCourses = examCourses;
            this.tutorialCourses = tutorialCourses;
            this.logger = logger;
        }
        public async Task<List<Course>> GetList(string login = null, bool? onlyActive = null)
        {
            List<Course> result = await CourseRepo.GetListAsync(login, onlyActive);
            return result;
        }
        public async Task<CourseDTO> GetDTO(int id)
        {
            Course course = await CourseRepo.GetAsync(id);
            return await GetDTO(course);
        } 
        public async Task<CourseDTO> GetDTO(Course course)
        {            
            CourseDTO result = new CourseDTO();
            List<exam> tempExams = new List<exam>();
            List<Tutorial> tempTutorials = new List<Tutorial>();
            var examsCourses = await examCourses.GetListAsync(course.Id);
            foreach (var ec in examsCourses)
            {
                exam e = null;
                try
                {
                    e = await ExamsRepo.GetAsync(ec.Id);
                    tempExams.Add(e);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Unknown Exam id: {ec.Id} *** try to delete *** {ex.ToString()}");
                    await examCourses.DeleteAsync(ec.CourseId, ec.Id);
                }
            }
            var tutorialsCourses = await tutorialCourses.GetListAsync(course.Id);
            foreach (var tc in tutorialsCourses)
            {
                Tutorial t = null;
                try
                {
                    t = await TutorialsRepo.GetAsync(tc.Id);
                    tempTutorials.Add(t);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Unknown Tutorial id: {tc.Id} *** try to delete *** {ex.ToString()}");
                    await tutorialCourses.DeleteAsync(tc.CourseId, tc.Id);
                }
            }
            result.Exams = tempExams;
            result.Tutorials = tempTutorials;
            result.Course = course;
            return result;
        } 
        public async Task<List<CourseDTO>> GetListDTO(string login = null, bool? onlyActive = null)
        {
            List<Course> courses = await CourseRepo.GetListAsync(login, onlyActive);
            var model = new List<CourseDTO>();
            foreach (var course in courses)
            {
                model.Add(await GetDTO(course));
            }
            return model;
        }

        public async Task<List<ExamContract.CourseModels.User>> GetMyCourses(string login, bool? onlyActive = null)
        {
            var myCourses = await UsersRepo.GetListAsync(login, onlyActive);
            myCourses.ForEach(async a =>
            {
                a.Course = await CourseRepo.GetAsync(a.CourseId);
            });
            return myCourses;
        }
        public async Task<UserCoursesDTO> GetUserCoursesAsync(string login, bool onlyActive)
        {
            var model = new UserCoursesDTO();
            model.MyCourses = await GetMyCourses(login, onlyActive);
            Thread.Sleep(50);
            model.AllCourses = await GetList(null, true);
            return model;
        }
    }
}
