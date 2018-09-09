# In this demo we are going to create som unit tests over a controller

* Remind that we are using DI Ninject

```C# SeriesController
using Backend.Contracts;
using Backend.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace TestingDemo.Controllers
{
    public class SeriesController : ApiController
    {
        private ISeriesRepository _repository;

        public SeriesController(ISeriesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("series/complete")]
        public IHttpActionResult RetriveCompleteSeries()
        {
            var result = _repository.GetSeries()
                .AsQueryable()
                .Where(s => s.Complete);
            
            return Ok(result);
        }

        [HttpGet]
        [Route("series/complete/titles")]
        public IHttpActionResult RetriveCompleteSeriesTitles()
        {
            // Version 2 Read all Serie table fields and we only want a subset
            var result = _repository.GetSeries()
                .AsQueryable()
                .Where(s => s.Complete)
                .Select(s => new { s.Title })
                .ToList()
                .Select(s => new Serie { Title = s.Title });

            return Ok(result);
        }

        [HttpGet]
        [Route("series")]
        public IHttpActionResult RetrieveAllSeries()
        {
            var result = _repository.GetSeries();
            if (result == null)
            {
                return Ok("no series to display"); // Refactor this test.
            }
            return Ok(result);
        }
        
        [HttpGet]
        [Route("series/{id}")]
        public async Task<IHttpActionResult> RetrieveSerieById(int id)
        {
            var serie = await _repository.GetSerieByIdAsync(id);
            if (serie == null)
            {
                return NotFound();
            }

            return Ok(serie);
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
        }
    }
}

```

* Show definitions.
* Explain naming pattern.
>TODO: Create builder pattern.

### 1. First lets add some tests related RetrieveAllSeries

```C#
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
    var contentResult = actionResult as OkNegotiatedContentResult<string>; // [1]
    
    // Assert
    Assert.IsNotNull(contentResult);
    Assert.IsNotNull(contentResult.Content);
    Assert.AreEqual(contentResult.Content, "no series to display"); // [2]
}
```
1. Remind that IHttpActionResult represents an async operation. That async operation is going to be handle by web api. We need to cast to `OkNegotiatedContentResult`, so we can get to results.

2. This is not a good idea. The tests get harder to mantain. Use a status code instead a string.

### 2. Ok, lets add some tests that involve get a Task

```C#
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
    Assert.IsNotInstanceOfType(actionResult, typeof(OkResult)); // [1]
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
```

* It's very close to the previous code.
1. This is good idea because we are asserting against a type.
2. All of these tests are of value type tests.
> NOTE: Include state test (exception)