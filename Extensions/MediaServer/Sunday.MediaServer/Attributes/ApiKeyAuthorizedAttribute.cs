using Microsoft.AspNetCore.Mvc;
using Sunday.MediaServer.Filters;

namespace Sunday.MediaServer.Attributes
{
    public class ApiKeyAuthorizedAttribute : TypeFilterAttribute
    {
        public ApiKeyAuthorizedAttribute() : base(typeof(ApiKeyAuthorizedFilter))
        {
        }
    }
}
