using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Series.API.web.Controllers;
using Series.Backend.Contracts;
using Series.Backend.Entities;

namespace Series.API.web.Tests
{
    [TestClass]
    public class SeriesControllerUnitTests
    {
        [TestMethod]
        public void GetAllSeries_returns_series_titles()
        {
            // Arrange
            var containerRepositoriesMock = new Mock<IContainerRepositories>();
            containerRepositoriesMock.Setup(c => c.SeriesRepository.GetSeries())
                .Returns
                (
                    new List<TVSerie>
                    {
                        new TVSerie { Title = "Title A" },
                    }
                );
            var containerRepositories = containerRepositoriesMock.Object;
            var seriesController = new SeriesController(containerRepositories);

            // Act
            var result = seriesController.GetAllSeries();

            // Assert
            Assert.AreEqual(result.ToList().Count, 1);
            Assert.AreEqual(result.ToList()[0], "Title A");
        }

        [TestMethod]
        public void GetSerie_with_title_no_exists_returns_not_found()
        {
            // Arrange
            var containerRepositoriesMock = new Mock<IContainerRepositories>();
            containerRepositoriesMock.Setup(c => c.SeriesRepository.GetSeries())
                .Returns(new List<TVSerie>());
            var containerRepositories = containerRepositoriesMock.Object;
            var seriesController = new SeriesController(containerRepositories);

            // Act 
            var result = seriesController.GetSerie("Test");

            // Assert
            Assert.IsInstanceOfType(result, typeof(System.Web.Http.Results.NotFoundResult));
        }

        [TestMethod]
        public void GetSerie_with_title_exists_returns_serie()
        {
            // Arrange
            var containerRepositoriesMock = new Mock<IContainerRepositories>();
            containerRepositoriesMock.Setup(c => c.SeriesRepository.GetSeries())
                .Returns(new List<TVSerie> { new TVSerie { Title = "Test" } });
            var containerRepositories = containerRepositoriesMock.Object;
            var seriesController = new SeriesController(containerRepositories);

            // Act 
            var result = (System.Web.Http.Results.OkNegotiatedContentResult<TVSerie>)seriesController.GetSerie("Test");

            // Assert
            Assert.AreEqual(result.Content.Title, "Test");
        }
    }
}
