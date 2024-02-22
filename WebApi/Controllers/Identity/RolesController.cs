using Application.Features.Identity.Roles.Commands;
using Application.Features.Identity.Roles.Queries;
using Common.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;
using WebApi.Controllers.Common;

namespace WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    public class RolesController(IMediator mediator) : BaseController<RolesController>
    {

        [HttpPost]
        [HasPermission(AppFeature.Roles, AppAction.Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRole(CreateRoleCommand request)
        {
            var response = await mediator.Send(request);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        [HasPermission(AppFeature.Roles, AppAction.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRoles()
        {
            var response = await mediator.Send(new GetRolesQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("{Id}")]
        [HasPermission(AppFeature.Roles, AppAction.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRoles(string Id)
        {
            var response = await mediator.Send(new GetRoleByIdQuery
            {
                RoleId = Id
            });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        [HasPermission(AppFeature.Roles, AppAction.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRole(UpdateRoleCommand request)
        {
            var response = await mediator.Send(request);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("{Id}")]
        [HasPermission(AppFeature.Roles, AppAction.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRoles(string Id)
        {
            var response = await mediator.Send(new DeleteRoleCommand
            {
                RoleId = Id
            });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("permissions/{RoleId}")]
        [HasPermission(AppFeature.RoleClaims, AppAction.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPermissions(string RoleId)
        {
            var response = await mediator.Send(new GetPermissionsQuery
            {
                RoleId = RoleId
            });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update-permissions")]
        [HasPermission(AppFeature.RoleClaims, AppAction.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRolePermissions(UpdateRolePermissionsCommand request)
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
