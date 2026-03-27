using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameLogic
{
    [System.Serializable]
    public class SaveData
    {
        [JsonProperty("version")]
        public int version = 1;

        [JsonProperty("saveTime")]
        public string saveTime;

        [JsonProperty("saveName")]
        public string saveName;

        [JsonProperty("progress")]
        public GameProgressData progress;

        [JsonProperty("player")]
        public PlayerStateData player;
    }

    [System.Serializable]
    public class GameProgressData
    {
        [JsonProperty("currentChapter")]
        public string currentChapter = "Chapter1";

        [JsonProperty("currentLevel")]
        public string currentLevel = "Level1";

        [JsonProperty("levelIndex")]
        public int levelIndex = 0;
    }

    [System.Serializable]
    public class PlayerStateData
    {
        [JsonProperty("health")]
        public int health = 100;

        [JsonProperty("maxHealth")]
        public int maxHealth = 100;

        [JsonProperty("energy")]
        public int energy = 50;

        [JsonProperty("maxEnergy")]
        public int maxEnergy = 50;

        [JsonProperty("skills")]
        public List<SkillSaveData> skills = new List<SkillSaveData>();
    }

    [System.Serializable]
    public class SkillSaveData
    {
        [JsonProperty("skillId")]
        public string skillId;

        [JsonProperty("level")]
        public int level = 1;

        [JsonProperty("isUnlocked")]
        public bool isUnlocked = true;
    }

    public class SaveDataExtensions
    {
        public static SaveData CreateNew(string saveName = "New Save")
        {
            return new SaveData
            {
                version = 1,
                saveTime = System.DateTime.UtcNow.ToString("o"),
                saveName = saveName,
                progress = new GameProgressData(),
                player = new PlayerStateData
                {
                    health = 100,
                    maxHealth = 100,
                    energy = 50,
                    maxEnergy = 50,
                    skills = new List<SkillSaveData>()
                }
            };
        }
    }
}
