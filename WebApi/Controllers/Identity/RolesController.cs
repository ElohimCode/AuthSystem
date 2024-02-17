using Application.Features.Identity.Roles.Commands;
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
    }
}
