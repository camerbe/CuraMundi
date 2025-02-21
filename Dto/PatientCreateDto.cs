using CuraMundi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuraMundi.Application.BLL.Dto
{
    public class PatientCreateDto
    {
        public Guid Id { get; set; }
        public required DateTime DateNaiss { get; set; }
        public string? Adresse { get; set; }
        public required string Role { get; set; } = "Patient";
        public required string Nom { get; set; }
        public required string Prenom { get; set; }
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? Telephone { get; set; }
    }
}
