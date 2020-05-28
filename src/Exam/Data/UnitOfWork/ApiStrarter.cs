using Exam.Services;
using ExamContract.CourseModels;
using ExamContract.TutorialModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam = ExamContract.MainDbModels.Exam;

namespace Exam.Data.UnitOfWork
{
    public class ApiStrarter
    {
        private readonly WebApiClient<exam> exams;
        private readonly WebApiClient<Course> courses;
        private readonly WebApiClient<Tutorial> tutorials;
        private readonly EmailSender emailSender;

        public ApiStrarter(WebApiClient<exam> exams, WebApiClient<Course> courses, WebApiClient<Tutorial> tutorials, EmailSender emailSender)
        {
            this.exams = exams;
            this.courses = courses;
            this.tutorials = tutorials;
            this.emailSender = emailSender;
        }
        public async Task WakeUp()
        {
            try
            {
                var e = await exams.WakeUp();
                var c = await courses.WakeUp();
                var t = await tutorials.WakeUp();
                var m = await emailSender.WakeUp();
            }
            catch 
            {
            }
        }
    }
}
