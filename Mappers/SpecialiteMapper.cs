using CuraMundi.Application.BLL.Dto;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Extensions;

namespace CuraMundi.Mappers
{
    public static class SpecialiteMapper
    {
        public static Specialite ToSpecialite(this SpecialiteCreateDto specialiteCreateDto)
        {
            return new Specialite
            {
                Titre = specialiteCreateDto.Titre.Capitalize(),
                
            };
            
        }
        public static SpecialiteDto ToSpecialiteDto(this Specialite specialite)
        {
            return new SpecialiteDto
            {
                Id = specialite.Id,
                Titre = specialite.Titre.Capitalize(),
            };
        }
    }
}
