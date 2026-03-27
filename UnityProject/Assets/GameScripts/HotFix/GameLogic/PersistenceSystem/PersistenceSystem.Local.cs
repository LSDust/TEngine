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
                    Log.Error($"No file name registered for type: {typeof(T).Name}");
                    return false;
                }
                
                string saveFilePath = Path.Combine(PersistencePath, fileName);
                Log.Info($"开始加载数据: {saveFilePath}");
                string jsonData = LocalFileIO.ReadStringFromFile(saveFilePath);

                if (string.IsNullOrEmpty(jsonData))
                {
                    Log.Warning($"File is empty or does not exist: {saveFilePath}");
                    return false;
                }

                data = Utility.Json.ToObject<T>(jsonData);
                if (data == null)
                {
                    Log.Error($"Failed to deserialize data of type: {typeof(T).Name}");
                    return false;
                }

                Log.Info($"Loaded data successfully: {saveFilePath}");
                return true;
            }
            catch (System.Exception e)
            {
                Log.Error($"Error loading data: {e.Message}");
                return false;
            }
        }

        public static bool SaveLocalData<T>(T data) where T : IPersistenceData, new()
        {
            try
            {
                if (data == null)
                {
                    Log.Error("Data is null.");
                    return false;
                }

                if (!FileNameDic.TryGetValue(typeof(T), out string fileName))
                {
                    Log.Error($"No file name registered for type: {typeof(T).Name}");
                    return false;
                }

                string saveFilePath = Path.Combine(PersistencePath, fileName);
                Log.Info($"开始保存数据: {saveFilePath}");
                string jsonData = Utility.Json.ToJson(data);

                if (string.IsNullOrEmpty(jsonData))
                {
                    Log.Error($"Failed to serialize data of type: {typeof(T).Name} to JSON.");
                    return false;
                }

                bool writeResult = LocalFileIO.WriteStringToFile(saveFilePath, jsonData);
                if (!writeResult)
                {
                    Log.Error($"Failed to write file: {saveFilePath}");
                    return false;
                }

                Log.Info($"Saved data successfully: {saveFilePath}");
                return true;
            }
            catch (System.Exception e)
            {
                Log.Error($"Error saving data: {e.Message}");
                return false;
            }
        }
    }
}
