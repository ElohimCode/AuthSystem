using Application.Features.Identity.User.Commands;
using Application.Features.Identity.User.Queries;
using Common.Requests.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Common;

namespace WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : BaseController<UserController>
    {
        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginQuery request)
        {
            var response = await mediator.Send(request);
            if(response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> GetRefreshToken(RefreshTokenQuery request)
        {
            var response = await mediator.Send(request);
            if(response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(UserRegistrationCommand request)
        {
            var response = await mediator.Send(request);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
