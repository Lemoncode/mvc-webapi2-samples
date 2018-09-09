using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Backend.Contracts;
using Backend.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestingDemo.Controllers;

namespace TestingDemo.Tests
{
    [TestClass]
    public class SeriesControllerUnitTests
    {
        // TODO: Test controller itself
        // TODO: Try dynamic
        [TestMethod]
        [Description("")]
        public void RetrieveAllSeries_When_Is_Invoked_Return_All_Series()
        {
            // Arrange
            var series = new List<Serie> { new Serie { Id = 1 } };
            var mock = new Mock<ISeriesRepository>();
            mock.Setup(s => s.GetSeries()).Returns(series);
            var controller = new SeriesController(mock.Object);

            // Act 
            var actionResult = controller.RetrieveAllSeries();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<Serie>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(contentResult.Content.ToList()[0].Id, 1);
        }

        // TODO: Try dynamic
        [TestMethod]
        [Description("")]
        public void RetrieveAllSeries_When_Is_Invoked_Return_No_Series()
        {
            // Arrange
            var mock = new Mock<ISeriesRepository>();
            mock.Setup(s => s.GetSeries()).Returns(() => null);
            var controller = new SeriesController(mock.Object);

            // Act 
            var actionResult = controller.RetrieveAllSeries();
            var contentResult = actionResult as OkNegotiatedContentResult<string>;
            
            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(contentResult.Content, "no series to display");
        }

        [TestMethod]
        [Description("")]
        public async Task RetrieveRetrieveSerieByIdSerie_With_Existing_Id_Returns_Ok()
        {
            // Arrange
            var mock = new Mock<ISeriesRepository>();
            mock.Setup(f => f.GetSerieByIdAsync(It.IsAny<int>()))
                .Returns(() => Task.Factory.StartNew(() => new Serie { Id = 1 }));
            var controller = new SeriesController(mock.Object);

            // Act
            var actionResult = await controller.RetrieveSerieById(1);

            // Assert
            Assert.IsNotInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        [Description("")]
        public async Task RetrieveSerieByIdSerie_With_Existing_Id_Returns_Content_Serie()
        {
            // Arrange
            var mock = new Mock<ISeriesRepository>();
            mock.Setup(f => f.GetSerieByIdAsync(It.IsAny<int>()))
                .Returns(() => Task.Factory.StartNew(() => new Serie { Id = 1 }));
            var controller = new SeriesController(mock.Object);

            // Act
            var actionResult = await controller.RetrieveSerieById(1);
            var contentResult = actionResult as OkNegotiatedContentResult<Serie>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.Content.Id, 1);
        }

        [TestMethod]
        [Description("")]
        public async Task RetrieveSerieByIdSerie_With_No_Existing_Id_Returns_NotFound()
        {
            // Arrange
            var mock = new Mock<ISeriesRepository>();
            mock.Setup(f => f.GetSerieByIdAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult<Serie>(null));
            var controller = new SeriesController(mock.Object);

            // Act
            var actionResult = await controller.RetrieveSerieById(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
    }
}
