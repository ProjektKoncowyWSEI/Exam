using Exam.Controllers;
using Exam.Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Exam.Tests.Controllers
{
    class HomeControllerTests
    {
        HomeController controller;
        [SetUp]
        public void Setup()
        {
            Mock<IStringLocalizer<SharedResource>> localizerMock = new Mock<IStringLocalizer<SharedResource>>();
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<ApiStrarter> apiStrarterMock = new Mock<ApiStrarter>();
            controller = new HomeController(localizerMock.Object, loggerMock.Object, apiStrarterMock.Object);
        }

        [Test]
        public void Index_ResultsIsNotNull()
        {
            var result = controller.Index().Result as ViewResult;
            Assert.IsNotNull(result);
        }
        [Test]
        public void Index_ResultsIndexView()
        {
            var result = controller.Index().Result as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
