using System.ComponentModel.DataAnnotations;

namespace CuraMundi.Dto
{
    public class PatientDetailDto
    {
        public Guid Id { get; set; }
        public required DateTime DateNaiss { get; set; }
        public string? Adresse { get; set; }
        public required string Role { get; set; } 
        public required string Nom { get; set; }
        public required string Prenom { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string FullName { get; set; }
    }
}
