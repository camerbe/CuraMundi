namespace CuraMundi.Dto
{
    public class UserDetailDto
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }

        public string Prenom { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }
}
