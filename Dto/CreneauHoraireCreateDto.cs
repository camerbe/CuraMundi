using CuraMundi.Domain.Entities;

namespace CuraMundi.Dto
{
    public class CreneauHoraireCreateDto
    {
        public DateTime Date { get; set; }
        public TimeSpan Debut { get; set; }
        public TimeSpan Fin { get; set; }
        public string Statut { get; set; }
        public Guid MedecinId { get; set; }
               
    }
}
