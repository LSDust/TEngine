using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TEngine;
using Log = TEngine.Log;

namespace GameLogic
{
    public static partial class PersistenceSystem
    {
        private const string SAVE_FOLDER_NAME = "PersistenceSystem";
        private static string PersistencePath => Path.Combine(Application.persistentDataPath, SAVE_FOLDER_NAME);

        private static readonly Dictionary<Type, string> FileNameDic = new()
        {
            { typeof(SaveData), "save.json" }
        };

        public static bool TryLoadLocalData<T>(out T data) where T : IPersistenceData, new()
        {
            data = default;
            try
            {
                if (!FileNameDic.TryGetValue(typeof(T), out string fileName))
                {
                    Log.Error($"类型 {typeof(T).Name} 未注册文件名");
                    return false;
                }
                
                string saveFilePath = Path.Combine(PersistencePath, fileName);
                Log.Info($"开始加载数据: {saveFilePath}");
                string jsonData = LocalFileIO.ReadStringFromFile(saveFilePath);

                if (string.IsNullOrEmpty(jsonData))
                {
                    Log.Warning($"文件为空或不存在: {saveFilePath}");
                    return false;
                }

                data = Utility.Json.ToObject<T>(jsonData);
                if (data == null)
                {
                    Log.Error($"反序列化数据失败,类型: {typeof(T).Name}");
                    return false;
                }

                Log.Info($"加载数据成功: {saveFilePath}");
                return true;
            }
            catch (System.Exception e)
            {
                Log.Error($"加载数据出错: {e.Message}");
                return false;
            }
        }

        public static bool SaveLocalData<T>(T data) where T : IPersistenceData, new()
        {
            try
            {
                if (data == null)
                {
                    Log.Error("数据为null。");
                    return false;
                }

                if (!FileNameDic.TryGetValue(typeof(T), out string fileName))
                {
                    Log.Error($"类型 {typeof(T).Name} 未注册文件名");
                    return false;
                }

                string saveFilePath = Path.Combine(PersistencePath, fileName);
                Log.Info($"开始保存数据: {saveFilePath}");
                string jsonData = Utility.Json.ToJson(data);

                if (string.IsNullOrEmpty(jsonData))
                {
                    Log.Error($"序列化数据失败,类型: {typeof(T).Name} 无法转为JSON。");
                    return false;
                }

                bool writeResult = LocalFileIO.WriteStringToFile(saveFilePath, jsonData);
                if (!writeResult)
                {
                    Log.Error($"写入文件失败: {saveFilePath}");
                    return false;
                }

                Log.Info($"保存数据成功: {saveFilePath}");
                return true;
            }
            catch (System.Exception e)
            {
                Log.Error($"保存数据出错: {e.Message}");
                return false;
            }
        }
    }
}
