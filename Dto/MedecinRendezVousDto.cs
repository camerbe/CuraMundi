using CuraMundi.Domain.Entities;

namespace CuraMundi.Dto
{
    public class MedecinRendezVousDto 
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public RendezVous RendezVous { get; set; }
        public PatientDetailDto Patient { get; set; }
    }
}
