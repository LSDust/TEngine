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
                    Log.Error("文件路径为空或null。");
                    return false;
                }

                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    Log.Info($"创建目录: {directory}");
                }

                File.WriteAllText(filePath, data);
                Log.Info($"保存文件成功: {filePath}");
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"写入文件失败: {e.Message}");
                return false;
            }
        }
        
        public static string ReadStringFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Log.Warning($"文件不存在: {filePath}");
                    return string.Empty;
                }

                return File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Log.Error($"读取文件失败: {e.Message}");
                return string.Empty;
            }
        }
        
        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Log.Warning($"文件不存在,无法删除: {filePath}");
                    return false;
                }

                File.Delete(filePath);
                Log.Info($"删除文件成功: {filePath}");
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"删除文件失败: {e.Message}");
                return false;
            }
        }
    }
}
