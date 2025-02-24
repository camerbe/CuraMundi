namespace CuraMundi.Dto
{
    public class RendezVousDetailDto
    {
        public Guid Id { get; set; }
        public TimeSpan Fin { get; set; }
        public TimeSpan Debut { get; set; }
        public DateTime DateRdv { get; set; }
        public string Statut { get; set; }
        public PatientDetailDto Patient { get; set; }
        public MedecinDetailDto Medecin { get; set; }
      
    }
}
