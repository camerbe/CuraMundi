using CuraMundi.Domain.Entities;

namespace CuraMundi.Dto
{
    public class CreneauHoraireDetailDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Debut { get; set; }
        public TimeSpan Fin { get; set; }
        public string Statut { get; set; }
        public MedecinDetailDto Medecin { get; set; }
    }
}
