using Series.Backend.Contracts;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Series.API.web.Filters
{
    public class ValidateUserFilter : IAuthorizationFilter
    {
        public bool AllowMultiple => true;

        private readonly string _userType;
        private readonly ISecurityRepository _securityRepository;

        public ValidateUserFilter(ISecurityRepository securityRepository, string userType)
        {
            _securityRepository = securityRepository;
            _userType = userType;
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var authToken = actionContext.Request.Headers.GetValues("Authorization-Token");
            var userId = ExtractUserId(actionContext.Request.RequestUri.Query);
            if (authToken != null && userId != null)
            {
                var userToken = _securityRepository.GetLastUserToken(int.Parse(userId));
                if (authToken.ToList()[0] == userToken)
                {
                    return continuation();
                }
                else
                {
                    Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden));
                }
            }
            
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden));
        }

        private string ExtractUserId(string requestQuery)
        {
            return requestQuery.StartsWith("?userId") ? requestQuery.Split('=')[1] : null;
        } 
    }
}