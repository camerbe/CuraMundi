using CuraMundi.Domain.Entities;
using CuraMundi.Dto;

namespace CuraMundi.Mappers
{
    public static class CreneauHoraireMapper
    {
        public static CrenauxHoraire ToCreneauHoraire(this CreneauHoraireCreateDto creneauHoraireCreateDto)
        {
            return new CrenauxHoraire
            {
                Date = creneauHoraireCreateDto.Date,
                Debut = creneauHoraireCreateDto.Debut,
                Fin = creneauHoraireCreateDto.Fin,
                Statut = creneauHoraireCreateDto.Statut,
                MedecinId = creneauHoraireCreateDto.MedecinId

            };
        }
        public static CreneauHoraireDetailDto ToCreneauHoraireDto(this CrenauxHoraire crenauxHoraire)
        {
            return new CreneauHoraireDetailDto
            {
                Id= crenauxHoraire.Id,
                Date = crenauxHoraire.Date,
                Debut = crenauxHoraire.Debut,
                Fin = crenauxHoraire.Fin,
                Statut = crenauxHoraire.Statut,
                Medecin = crenauxHoraire.Medecin.ToMedecinDto()

            };
        }

    }
    
}
