using System.ComponentModel.DataAnnotations;

namespace CuraMundi.Dto
{
    public class LoginDto
    {
        public required string Email { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
