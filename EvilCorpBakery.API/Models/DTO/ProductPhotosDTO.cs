namespace EvilCorpBakery.API.Models.DTO
{
    public class ProductPhotosDTO
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool isMain { get; set; }
        }
}
