using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Series.API.web.Filters;
using Series.Backend.Contracts;

namespace Series.API.web.Tests
{
    [TestClass]
    public class ValidateUserFilterUnitTests
    {
        private Func<Task<HttpResponseMessage>> ok = async () => await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        
        [TestMethod]
        public async Task ExecuteAuthorizationFilterAsync_When_AuthorizationToken_Is_Not_Informed_Resolves_Forbidden()
        {
            // Arrange
            var securityRepositoryMock = new Mock<ISecurityRepository>();
            securityRepositoryMock.Setup(s => s.GetLastUserToken(It.IsAny<int>()))
                .Returns("token");
            var securityRepository = securityRepositoryMock.Object;
            var uri = new Uri("http://localhost:62608/api/userseries?userId=1");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Add("Authorization-Token", "");
            var routeDataMock = new Mock<IHttpRouteData>();
            var actionDescriptorMock = new Mock<HttpActionDescriptor>();
            var actionContext = new HttpActionContext
                (
                    new HttpControllerContext(new HttpConfiguration(), routeDataMock.Object, request),
                    actionDescriptorMock.Object
                );
            CancellationTokenSource source = new CancellationTokenSource();
            var validateUserFilter = new ValidateUserFilter(securityRepository, null);

            // Act 
            var result = await validateUserFilter.ExecuteAuthorizationFilterAsync(actionContext, source.Token, ok);

            // Assert
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public async Task ExecuteAuthorizationFilterAsync_When_User_Has_Not_Token_Resolves_Forbidden()
        {
            // Arrange
            var securityRepositoryMock = new Mock<ISecurityRepository>();
            securityRepositoryMock.Setup(s => s.GetLastUserToken(It.IsAny<int>()))
                .Returns("");
            var securityRepository = securityRepositoryMock.Object;
            var uri = new Uri("http://localhost:62608/api/userseries?userId=1");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Add("Authorization-Token", "token");
            var routeDataMock = new Mock<IHttpRouteData>();
            var actionDescriptorMock = new Mock<HttpActionDescriptor>();
            var actionContext = new HttpActionContext
                (
                    new HttpControllerContext(new HttpConfiguration(), routeDataMock.Object, request),
                    actionDescriptorMock.Object
                );
            CancellationTokenSource source = new CancellationTokenSource();
            var validateUserFilter = new ValidateUserFilter(securityRepository, null);

            // Act 
            var result = await validateUserFilter.ExecuteAuthorizationFilterAsync(actionContext, source.Token, ok);

            // Assert
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public async Task ExecuteAuthorizationFilterAsync_When_User_Has_Valid_Token_And_AuthorizationToken_Is_Informed()
        {
            // Arrange
            var securityRepositoryMock = new Mock<ISecurityRepository>();
            securityRepositoryMock.Setup(s => s.GetLastUserToken(It.IsAny<int>()))
                .Returns("token");
            var securityRepository = securityRepositoryMock.Object;

            var uri = new Uri("http://localhost:62608/api/userseries?userId=1");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Add("Authorization-Token", "token");

            var routeDataMock = new Mock<IHttpRouteData>();
            var actionDescriptorMock = new Mock<HttpActionDescriptor>();
            var actionContext = new HttpActionContext
                (
                    new HttpControllerContext(new HttpConfiguration(), routeDataMock.Object, request),
                    actionDescriptorMock.Object
                );
            CancellationTokenSource source = new CancellationTokenSource();
            var validateUserFilter = new ValidateUserFilter(securityRepository, null);

            // Act 
            var result = await validateUserFilter.ExecuteAuthorizationFilterAsync(actionContext, source.Token, ok);

            // Assert
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }
    }
}
