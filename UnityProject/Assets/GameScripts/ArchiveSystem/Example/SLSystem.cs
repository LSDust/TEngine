using System;
using System.Collections;
using System.Collections.Generic;
using NuoYan.Archive;
using UnityEngine;

[Serializable]
public struct RoleData
{
    public string name;
    public string id;
    public int level;
}

[Serializable]
public struct GameProgressData
{
    public string current_scene;
}

[Serializable]
public class GameData : ArchiveDataBase
{
    public RoleData roleData;
    public GameProgressData gameProgressData;
}

[Serializable]
public class ArchiveCatalog : ArchiveTableBase
{
    public string name;
}

public class SLSystem : ArchiveSystem<GameData, ArchiveCatalog>
{
    [ContextMenu("Save")]
    public void SaveTest()
    {
        Save();
    }
    
    [ContextMenu("Load")]
    public void LoadTest()
    {
        Load();
    }
}
