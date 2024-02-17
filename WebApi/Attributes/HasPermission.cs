using Common.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Attributes
{
    public class HasPermission : AuthorizeAttribute
    {
        public HasPermission(string feature, string action)
        => Policy = AppPermission.NameFor(feature, action);
    }
}
