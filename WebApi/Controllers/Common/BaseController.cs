using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Common
{
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
    }
}
