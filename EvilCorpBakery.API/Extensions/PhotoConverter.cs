using System;
using System.IO;
using System.Threading.Tasks;

namespace EvilCorpBakery.API.Extensions
{
    public class PhotoConverter
    {
        public static async Task<string> ConvertImageToBase64Async(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File not found: {filePath}");
                }

                string extension = Path.GetExtension(filePath).ToLowerInvariant();
                if (!IsValidImageFormat(extension))
                {
                    throw new ArgumentException($"Unsupported file format: {extension}");
                }

                byte[] imageBytes = await File.ReadAllBytesAsync(filePath);

                string base64String = Convert.ToBase64String(imageBytes);

                return $"data:image/{GetMimeType(extension)};base64,{base64String}";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error during image conversion: {ex.Message}", ex);
            }
        }

        private static bool IsValidImageFormat(string extension)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            return Array.Exists(validExtensions, ext => ext == extension);
        }

        private static string GetMimeType(string extension)
        {
            return extension switch
            {
                ".jpg" or ".jpeg" => "jpeg",
                ".png" => "png",
                ".gif" => "gif",
                ".bmp" => "bmp",
                ".webp" => "webp",
                _ => "jpeg" // default
            };
        }
    }
}