using System.Web.Http.Filters;

namespace Series.API.web.Filters
{
    public class ValidateUserAttribute : FilterAttribute
    {
        public string UserType { get; private set; }

        public ValidateUserAttribute(string userType)
        {
            UserType = userType;
        }
    }
}