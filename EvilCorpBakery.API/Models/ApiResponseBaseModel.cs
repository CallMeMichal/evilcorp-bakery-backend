namespace EvilCorpBakery.API.Models
{
    public class ApiResponseBaseModel
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Detail { get; set; } = string.Empty;
        public string Instance { get; set; } = string.Empty;
    }

    public class ApiResponse<T> : ApiResponseBaseModel
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ApiResponse : ApiResponseBaseModel
    {
        public bool Success { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}