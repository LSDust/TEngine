using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Log = TEngine.Log;

namespace GameLogic
{
    public static class LocalFileIO
    {
        public static bool WriteStringToFile(string filePath, string data)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    Log.Error("File path is null or empty.");
                    return false;
                }

                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    Log.Info($"Created directory: {directory}");
                }

                File.WriteAllText(filePath, data);
                Log.Info($"Save file written successfully: {filePath}");
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"Failed to write file: {e.Message}");
                return false;
            }
        }

        public static async Task<bool> WriteStringToFileAsync(string filePath, string data)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    Log.Error("File path is null or empty.");
                    return false;
                }

                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    Log.Info($"Created directory: {directory}");
                }

                await File.WriteAllTextAsync(filePath, data);
                Log.Info($"Save file written asynchronously: {filePath}");
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"Failed to write file asynchronously: {e.Message}");
                return false;
            }
        }

        public static string ReadStringFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Log.Warning($"File does not exist: {filePath}");
                    return string.Empty;
                }

                return File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to read file: {e.Message}");
                return string.Empty;
            }
        }

        public static async Task<string> ReadStringFromFileAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Log.Warning($"File does not exist: {filePath}");
                    return string.Empty;
                }

                return await File.ReadAllTextAsync(filePath);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to read file asynchronously: {e.Message}");
                return string.Empty;
            }
        }

        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Log.Warning($"File does not exist, cannot delete: {filePath}");
                    return false;
                }

                File.Delete(filePath);
                Log.Info($"File deleted successfully: {filePath}");
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"Failed to delete file: {e.Message}");
                return false;
            }
        }

        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static void EnsureDirectoryExists(string directoryPath)
        {
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
