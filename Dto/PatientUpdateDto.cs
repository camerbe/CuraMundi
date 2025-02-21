using System.ComponentModel.DataAnnotations;

namespace CuraMundi.Dto
{
    public class PatientUpdateDto
    {
        //public Guid Id { get; set; }
        public  DateTime? DateNaiss { get; set; }
        public string? Adresse { get; set; }
        //public required string Role { get; set; } 
        public  string? Nom { get; set; }
        public  string? Prenom { get; set; }
        //public string? Email { get; set; }
        public string? Telephone { get; set; }
    }
}
