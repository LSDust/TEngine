using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using TEngine;
using Log = TEngine.Log;

namespace GameLogic
{
    public partial class PersistenceModule : Singleton<PersistenceModule>
    {
        private static readonly object SaveLock = new object();
        private const string SAVE_FOLDER_NAME = "PersistenceModule";
        private const string SAVE_FILE_NAME = "save.json";
        private static string PersistencePath => Path.Combine(Application.persistentDataPath, SAVE_FOLDER_NAME);

        public SaveData LoadLocalSaveData(string saveFileName = SAVE_FILE_NAME)
        {
            lock (SaveLock)
            {
                try
                {
                    string saveFilePath = Path.Combine(PersistencePath, saveFileName);
                    Log.Info($"开始加载存档数据: {saveFilePath}");
                    string jsonData = LocalFileIO.ReadStringFromFile(saveFilePath);

                    if (string.IsNullOrEmpty(jsonData))
                    {
                        Log.Warning($"Save file is empty or does not exist: {saveFilePath}");
                        return SaveDataExtensions.CreateNew();
                    }

                    SaveData saveData = Utility.Json.ToObject<SaveData>(jsonData);
                    if (saveData == null)
                    {
                        Log.Error("Failed to deserialize save data.");
                        return SaveDataExtensions.CreateNew();
                    }

                    Log.Info($"Loaded save data successfully: {saveFilePath}");
                    return saveData;
                }
                catch (System.Exception e)
                {
                    Log.Error($"Error loading save data: {e.Message}");
                    return SaveDataExtensions.CreateNew();
                }
            }
        }

        public string SaveLocalSaveData(SaveData saveData, string saveFileName = SAVE_FILE_NAME)
        {
            lock (SaveLock)
            {
                try
                {
                    if (saveData == null)
                    {
                        string errorMessage = "Save data is null.";
                        Log.Error(errorMessage);
                        return errorMessage;
                    }

                    string saveFilePath = Path.Combine(PersistencePath, saveFileName);
                    Log.Info($"开始保存存档数据: {saveFilePath}");
                    string jsonData = Utility.Json.ToJson(saveData);

                    if (string.IsNullOrEmpty(jsonData))
                    {
                        string errorMessage = "Failed to serialize save data to JSON.";
                        Log.Error(errorMessage);
                        return errorMessage;
                    }

                    bool writeResult = LocalFileIO.WriteStringToFile(saveFilePath, jsonData);
                    if (!writeResult)
                    {
                        string errorMessage = $"Failed to write save file: {saveFilePath}";
                        Log.Error(errorMessage);
                        return errorMessage;
                    }

                    Log.Info($"Saved save data successfully: {saveFilePath}");
                    return string.Empty;
                }
                catch (System.Exception e)
                {
                    string errorMessage = $"Error saving save data: {e.Message}";
                    Log.Error(errorMessage);
                    return errorMessage;
                }
            }
        }

        public async Task<SaveData> LoadLocalSaveDataAsync(string saveFileName = SAVE_FILE_NAME, CancellationToken cancellationToken = default)
        {
            Log.Info($"开始异步加载存档数据: {Path.Combine(PersistencePath, saveFileName)}");
            await Task.Yield();
            return LoadLocalSaveData(saveFileName);
        }

        public async Task<string> SaveLocalSaveDataAsync(SaveData saveData, string saveFileName = SAVE_FILE_NAME, CancellationToken cancellationToken = default)
        {
            Log.Info($"开始异步保存存档数据: {Path.Combine(PersistencePath, saveFileName)}");
            await Task.Yield();
            return SaveLocalSaveData(saveData, saveFileName);
        }
    }
}
