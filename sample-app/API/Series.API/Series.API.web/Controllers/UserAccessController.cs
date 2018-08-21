using Security.RestModels;
using Security.Utils;
using Security.Validators;
using Series.Backend.Contracts;
using System.Web.Http;

namespace Series.API.web.Controllers
{
    public class UserAccessController : ApiController
    {
        IContainerRepositories _containerRepositories;
        IHashGenerator _hashGenerator;
        IContentValidator _contentValidator;
        ITokenGenerator _tokenGenerator;
        
        public UserAccessController
            (
                IContainerRepositories containerRepositories,
                IHashGenerator hashGenerator,
                IContentValidator contentValidator,
                ITokenGenerator tokenGenerator
            )
        {
            _containerRepositories = containerRepositories;
            _hashGenerator = hashGenerator;
            _contentValidator = contentValidator;
            _tokenGenerator = tokenGenerator;
        }

        //http://localhost:62608/api/useraccess/signin
        [HttpPost]
        public IHttpActionResult SignIn([FromBody]UserCredentials userCredentials)
        {
            // TODO: Use newtosoft instead
            var securityRepository = _containerRepositories.SecurityRepository;
            var userProfile = securityRepository.GetUserProfileByUserEmail(userCredentials.Email);

            if (userProfile != null)
            {
                var expectedPasswordHash = _hashGenerator.SaltedContentHash(userCredentials.Password, userProfile.Salt);
                if (expectedPasswordHash == userProfile.PasswordHash)
                {
                    var token = _tokenGenerator.GenerateToken();
                    securityRepository.CreateUserSession(token, userProfile.Id);
                    return Ok(token);
                }
            }

            return Unauthorized();
        }
    }
}
