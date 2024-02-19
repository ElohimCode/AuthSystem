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
        public async Task<IActionResult> GetRoles(string Id)
        {
            var response = await mediator.Send(new GetRoleByIdQuery
            {
                roleId = Id
            });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        [HasPermission(AppFeature.Roles, AppAction.Update)]
        public async Task<IActionResult> UpdateRole(UpdateRoleCommand request)
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
