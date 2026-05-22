using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgressArchive : SLSystem.IArchive
{
    public GameProgressData gameProgressData = new();
    
    public static GameProgressArchive Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        Instance = new GameProgressArchive();
        ((SLSystem.IArchive)Instance).Register(Instance);
        SceneManager.sceneLoaded += Instance.OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameProgressData.current_scene = scene.name;
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
