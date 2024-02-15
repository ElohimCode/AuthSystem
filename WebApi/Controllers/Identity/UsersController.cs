using Application.Features.Identity.User.Queries;
using Common.Requests.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Common;

namespace WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    public class UsersController : BaseController<UsersController>
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginQuery request)
        {
            var response = await _mediator.Send(request);
            if(response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
