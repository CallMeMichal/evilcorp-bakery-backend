namespace EvilCorpBakery.API.Models.Domain
{
    public class SelectedAddressDomain
    {
        public int? Id { get; set; }
        public string? Label { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PhoneAreaCode { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsDefault { get; set; }
    }
}
