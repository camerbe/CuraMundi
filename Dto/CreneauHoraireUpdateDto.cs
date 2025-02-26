namespace CuraMundi.Dto
{
    public class CreneauHoraireUpdateDto
    {
        public DateTime? Date { get; set; }
        public TimeSpan? Debut { get; set; }
        public TimeSpan? Fin { get; set; }
        public string? Statut { get; set; }
        public Guid? MedecinId { get; set; }
    }
}
