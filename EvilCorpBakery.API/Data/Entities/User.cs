namespace EvilCorpBakery.API.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // Default role is "User"
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>(); //virtual for lazy loading
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>(); //virtual for lazy loading


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
