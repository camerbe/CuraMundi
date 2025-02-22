using System.ComponentModel.DataAnnotations;

namespace CuraMundi.Dto
{
    public class MedecinCreateDto
    {
       
        public Guid Id { get; set; }
        public string? Inami { get; set; }
        public string? Adresse { get; set; }
        public required string Role { get; set; } 
        public required string Nom { get; set; }
        public required string Prenom { get; set; }
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? Telephone { get; set; }
        public Guid SpecialiteId { get; set; }
    }
}
