namespace EvilCorpBakery.API.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool isActive { get; set; }
        public List<AddressDTO>? Addresses { get; set; }
    }
}
