namespace EvilCorpBakery.API.Models.DTO
{
    public class AddressDTO
    {
        public int Id { get; set; }
        public string? Street { get; set; } 
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public bool IsDefault { get; set; }
        public string? Label { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneAreaCode { get; set; } = string.Empty;
    }
}
