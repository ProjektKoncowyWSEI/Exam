using Exam.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Exam.Tests.Controllers
{
    class AnswersControllerTests
    {
        AnswersController controller;

        [SetUp]
        public void Setup()
        {
            Mock<IStringLocalizer<SharedResource>> localizerMock = new Mock<IStringLocalizer<SharedResource>>();

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IConfiguration> configMock = new Mock<IConfiguration>();
            //var service = new AnswersApiClient(loggerMock.Object, configMock.Object);
            //Mock<AnswersApiClient> serviceMock = new Mock<AnswersApiClient>();
            controller = new AnswersController(localizerMock.Object, null, null); 
        }

        [Test]
        public void Create_ThrowException()
        {
            Assert.That(() => controller.Create(null), Throws.Exception);
        }      
    }
}
