using CuraMundi.Domain.Entities;

namespace CuraMundi.Dto
{
    public class MedecinDetailDto
    {
        public Guid Id { get; set; }
        public required string Inami { get; set; }
        public string? Adresse { get; set; }
        public required string Role { get; set; }
        public required string Nom { get; set; }
        public required string Prenom { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string FullName { get; set; }
        public SpecialiteDto Specialite { get; set; }
    }
}
