/***************************************************************************************
 * File Name    : FileHelper.cs
 * Description  : Provides file-related utility methods for saving, validating, and managing
 *                file uploads and downloads in the application.
 * 
 * Developer    : Aman Kala
 * Created Date : August 6, 2025
 * Last Modified: August 6, 2025
 * Version      : 1.0.0
 * 
 * Revision History:
 * -------------------------------------------------------------------------------------
 * Date        | Developer     | Description
 * -------------------------------------------------------------------------------------
 * 2025-08-06  | Aman Kala   | Initial creation of the FileHelper class.
 * 
 ****************************************************************************************/
namespace EMP.Helper
{
    public static class FileHelper
    {
        private static readonly string[] AllowedFileExtensions = { ".pdf", ".docx", ".xlsx", ".png", ".jpg", ".jpeg" };
        private const long MaxFileSizeInBytes = 10 * 1024 * 1024; // 10 MB

        /// <summary>
        /// Saves the uploaded file to disk with optional overwrite and returns the full path.
        /// </summary>
        public static async Task<string> SaveFileToDisk(IFormFile file, string destinationFolder, string? fileName = null)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null.");

            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            string safeFileName = fileName ?? GenerateUniqueFileName(file.FileName);
            string fullPath = Path.Combine(destinationFolder, safeFileName);

            using FileStream stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fullPath;
        }

        /// <summary>
        /// Validates the file based on size and extension.
        /// </summary>
        public static bool ValidateFile(IFormFile file, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (file == null || file.Length == 0)
            {
                errorMessage = "File is empty.";
                return false;
            }

            if (file.Length > MaxFileSizeInBytes)
            {
                errorMessage = "File size exceeds the maximum allowed size of 10MB.";
                return false;
            }

            string extension = Path.GetExtension(file.FileName)?.ToLower();
            if (!AllowedFileExtensions.Contains(extension))
            {
                errorMessage = $"File type '{extension}' is not allowed.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the file name contains only valid characters.
        /// </summary>
        public static bool IsValidFileName(string fileName)
        {
            // File name must not contain invalid characters or path traversal
            return !string.IsNullOrWhiteSpace(fileName) &&
                   !fileName.Any(Path.GetInvalidFileNameChars().Contains) &&
                   !fileName.Contains("..");
        }

        /// <summary>
        /// Checks if the file extension is among allowed types.
        /// </summary>
        public static bool IsValidFileType(string fileName)
        {
            string extension = Path.GetExtension(fileName)?.ToLower();
            return AllowedFileExtensions.Contains(extension);
        }

        /// <summary>
        /// Safely deletes a file if it exists.
        /// </summary>
        public static bool DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Generates a unique file name by trimming the original name to 8 characters
        /// and appending a unique 8-character ID, followed by the original extension.
        /// </summary>
        /// <param name="originalFileName">The original file name with extension.</param>
        /// <returns>A unique file name.</returns>
        public static string GenerateUniqueFileName(string originalFileName)
        {
            // Extract the file extension (e.g., ".pdf")
            string extension = Path.GetExtension(originalFileName);

            // Extract file name without extension and trim to 8 characters max
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
            string trimmedName = fileNameWithoutExt.Length > 8
                ? fileNameWithoutExt.Substring(0, 8)
                : fileNameWithoutExt;

            // Generate a short 8-character unique identifier from a GUID
            string shortGuid = Guid.NewGuid().ToString("N").Substring(0, 8);

            // Combine the trimmed name, short GUID, and extension
            return $"{trimmedName}_{shortGuid}{extension}";
        }
    }
}
