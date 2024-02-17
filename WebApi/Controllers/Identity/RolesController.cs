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
    public class RolesController : BaseController<RolesController>
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [HasPermission(AppFeature.Roles, AppAction.Create)]
        public async Task<IActionResult> CreateRole(CreateRoleCommand request)
        {
            var response = await _mediator.Send(request);
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
            var response = await _mediator.Send(new GetRolesQuery());
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
            var response = await _mediator.Send(new GetRoleByIdQuery
            {
                roleId = Id
            });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
