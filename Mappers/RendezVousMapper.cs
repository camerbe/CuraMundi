using CuraMundi.Application.BLL.Mappers;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;

namespace CuraMundi.Mappers
{
    public static class RendezVousMapper
    {
        public static RendezVous ToRendezVous(this RendezVousCreateDto rendezVousCreateDto)
        {
            return new RendezVous
            {
                Debut = rendezVousCreateDto.Debut,
                Fin = rendezVousCreateDto.Fin,
                DateRdv = rendezVousCreateDto.DateRdv,
                Statut = rendezVousCreateDto.Statut,
                MedecinId = rendezVousCreateDto.MedecinId,
                PatientId = rendezVousCreateDto.PatientId
            };
        }

        public static RendezVousDetailDto ToRendezVousDto(this RendezVous rendezVous)
        {
            return new RendezVousDetailDto
            {
                Id = rendezVous.Id,
                Fin = rendezVous.Fin,
                Debut = rendezVous.Debut,
                Statut = rendezVous.Statut,
                DateRdv = rendezVous.DateRdv,
                Medecin = rendezVous.Medecin.ToMedecinDto(),
                Patient = rendezVous.Patient.ToPatientDto(),
                 
            };
        }
    }
}
