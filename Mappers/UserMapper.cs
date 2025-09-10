using CuraMundi.Domain.Entities;
using CuraMundi.Dto;

namespace CuraMundi.Mappers
{
    public static class UserMapper
    {
        public static UserDetailDto ToUserDto(this User user)
        {
            return new UserDetailDto
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                Role = "Admin",
                Active = user.EmailConfirmed,
                 
            };
        }
    }
}
