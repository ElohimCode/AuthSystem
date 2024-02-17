using AutoMapper;
using Common.Responses.Identity;
using Persistence.Models;

namespace Infrastructure
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<ApplicationRole, RoleResponse>();

        }
    }
}
