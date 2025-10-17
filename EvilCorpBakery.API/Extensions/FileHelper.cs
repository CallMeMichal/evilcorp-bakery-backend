using EvilCorpBakery.API.Data.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EvilCorpBakery.API.Extensions
{
    public static class FileHelper
    {
        public static async Task<string> SaveBase64FileAsync(string base64Data, string folder, string fileName = null)
        {
            try
            {
                string originalBase64 = base64Data;

                if (base64Data.Contains(","))
                {
                    base64Data = base64Data.Split(',')[1];
                }
                byte[] fileBytes = Convert.FromBase64String(base64Data);

                if (string.IsNullOrEmpty(fileName))
                {
                    string extension = GetFileExtensionFromMimeType(originalBase64);
                    fileName = $"{Guid.NewGuid()}{extension}";
                }

                // TUTAJ BYŁA ZMIANA - dodaj wwwroot
                string fullFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);

                if (!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }
                string filePath = Path.Combine(fullFolderPath, fileName);
                await File.WriteAllBytesAsync(filePath, fileBytes);
                return $"/{folder}/{fileName}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not save file: {ex.Message}");
            }
        }

        public static async Task<string> ConvertImageToBase64Async(string filePath)
        {
            try
            {
                string fullPath;

                if (Path.IsPathRooted(filePath))
                {
                    fullPath = filePath;
                }
                else
                {
                    string cleanPath = filePath.TrimStart('/', '\\');
                    fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cleanPath);
                }

                if (!File.Exists(fullPath))
                {
                    fullPath = "wwwroot\\" + filePath.TrimStart('/', '\\');
                }

                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"File not found: {fullPath} (original: {filePath})");
                }

                string extension = Path.GetExtension(fullPath).ToLowerInvariant();
                if (!IsValidImageFormat(extension))
                {
                    throw new ArgumentException($"Unsupported file format: {extension}");
                }

                byte[] imageBytes = await File.ReadAllBytesAsync(fullPath);
                string base64String = Convert.ToBase64String(imageBytes);
                return $"data:image/{GetMimeTypeFromExtension(extension)};base64,{base64String}";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error during image conversion: {ex.Message}", ex);
            }
        }

        public static bool DeleteFile(string relativePath)
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string GetFileExtensionFromMimeType(string base64Data)
        {
            if (base64Data.Contains("data:image/png"))
                return ".png";
            else if (base64Data.Contains("data:image/jpeg") || base64Data.Contains("data:image/jpg"))
                return ".jpg";
            else if (base64Data.Contains("data:image/gif"))
                return ".gif";
            else if (base64Data.Contains("data:image/webp"))
                return ".webp";
            else if (base64Data.Contains("data:image/bmp"))
                return ".bmp";
            return ".jpg";
        }

        public static bool IsValidImageBase64(string base64Data)
        {
            if (string.IsNullOrEmpty(base64Data))
                return false;
            return base64Data.StartsWith("data:image/") && base64Data.Contains("base64,");
        }

        public static long GetFileSizeFromBase64(string base64Data)
        {
            if (base64Data.Contains(","))
            {
                base64Data = base64Data.Split(',')[1];
            }
            return (base64Data.Length * 3) / 4;
        }

        private static bool IsValidImageFormat(string extension)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            return Array.Exists(validExtensions, ext => ext == extension);
        }

        private static string GetMimeTypeFromExtension(string extension)
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