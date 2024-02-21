﻿using Common.Requests.Identity.Roles;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Commands
{
    public class UpdateRoleCommand : UpdateRoleRequest, IRequest<IResponseWrapper>
    {
    }
}
