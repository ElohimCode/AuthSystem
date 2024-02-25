using Application.Features.Identity.User.Commands;
using Application.Features.Identity.User.Queries;
using Common.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;
using WebApi.Controllers.Common;

namespace WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : BaseController<UserController>
    {
        [HttpPost("sign-in")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginQuery request)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserRegistrationCommand request)
        {
            var response = await mediator.Send(request);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        [HasPermission(AppFeature.Users, AppAction.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsers()
        {
            var response = await mediator.Send(new GetUsersQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("{Id}")]
        [HasPermission(AppFeature.Users, AppAction.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(string Id)
        {
            var response = await mediator.Send(new GetUserQuery
            {
                Id = Id
            });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete("{Id}")]
        [HasPermission(AppFeature.Users, AppAction.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var response = await mediator.Send(new DeleteUserCommand
            {
                Id = Id
            });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        
        [HttpPut]
        [HasPermission(AppFeature.Users, AppAction.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand request)
        {
            var response = await mediator.Send(request);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut("update-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserPassword(UpdateUserPasswordCommand request)
        {
            var response = await mediator.Send(request);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        
        [HttpGet("roles/{UserId}")]
        [HasPermission(AppFeature.Roles, AppAction.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserRoles(string UserId)
        {
            var response = await mediator.Send(new GetUserRolesQuery(UserId));
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
