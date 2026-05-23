using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgressArchive : SLSystem.IArchive
{
    public GameProgressData gameProgressData;
    
    public static GameProgressArchive Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        Instance = new GameProgressArchive();
        ((SLSystem.IArchive)Instance).Register(Instance);
        SceneManager.sceneLoaded += Instance.OnSceneLoaded;
        Instance.gameProgressData = new GameProgressData {current_scene = "PlayerRoom"};
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Loading")
        {
            gameProgressData.current_scene = scene.name;
        }
    }

    public void GetData(GameData data)
    {
        gameProgressData = data.gameProgressData;
    }

    public void SetData(GameData data)
    {
        data.gameProgressData = gameProgressData;
    }
}
