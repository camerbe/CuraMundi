using System.ComponentModel.DataAnnotations;

namespace CuraMundi.Dto
{
    public class MedecinUpdateDto
    {
        public string? Inami { get; set; }
        public  string? Nom { get; set; }
        public  string? Prenom { get; set; }
        public string? Telephone { get; set; }
        public Guid? SpecialiteId { get; set; }
    }
}
