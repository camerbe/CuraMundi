using System.ComponentModel.DataAnnotations;

namespace CuraMundi.Dto
{
    public class RendezVousUpdateDto
    {
        public TimeSpan? Debut { get; set; }
        public DateTime? DateRdv { get; set; }
        public TimeSpan? Fin { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? MedecinId { get; set; }
        public string? Statut { get; set; }
    }
}
