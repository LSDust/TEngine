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
public class GameData : ArchiveDataBase
{
    public RoleData roleData;
}

[Serializable]
public class ArchiveCatalog : ArchiveTableBase
{
    public string name;
}

public class SLTest : ArchiveSystem<GameData, ArchiveCatalog>
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
