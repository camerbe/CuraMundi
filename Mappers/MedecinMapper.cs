using CuraMundi.Application.BLL.Dto;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Extensions;

namespace CuraMundi.Mappers
{
    public static class MedecinMapper
    {
        public static Medecin ToMedecin(this MedecinCreateDto medecinCreateDto)
        {
            return new Medecin
            {
                Inami = medecinCreateDto.Inami,
                Email = medecinCreateDto.Email,
                UserName = medecinCreateDto.Email,
                Nom = medecinCreateDto.Nom.ToUpperCase(),
                Prenom = medecinCreateDto.Prenom.Capitalize(),
                SpecialiteId = medecinCreateDto.SpecialiteId
            };
        }

        public static MedecinDetailDto ToMedecinDto(this Medecin medecin)
        {
            return new MedecinDetailDto
            {
                Id = medecin.Id,
                Inami = medecin.Inami,
                Nom = medecin.Nom,
                Prenom = medecin.Prenom,
                Email = medecin.Email,
                Telephone = medecin.PhoneNumber,
                Role = "Medecin",
                FullName = medecin.Nom + " " + medecin.Prenom,
                Specialite = medecin.Specialite.ToSpecialiteDto() // Ensure ToSpecialiteDto() returns SpecialiteDto
            };
        }
    }
}
