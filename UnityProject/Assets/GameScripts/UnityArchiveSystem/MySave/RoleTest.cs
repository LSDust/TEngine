using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleTest : MonoBehaviour,SLTest.IArchive
{
    SLTest.IArchive archive;

    [SerializeField] public RoleData roleData = new();
    
    private void Awake()
    {
        archive = this;
    }

    private void OnEnable()
    {
        archive.Register(this);
    }

    private void OnDisable()
    {
        archive.Unregister(this);
    }

    public void GetData(GameData data)
    {
        this.roleData = data.roleData;
        // this.roleData.name = data.roleData.name;
        // this.roleData.id = data.roleData.id;
        // this.roleData.level = data.roleData.level;
    }

    public void SetData(GameData data)
    {
        data.roleData = roleData;
        // data.roleData.name = roleData.name;
        // data.roleData.id = roleData.id;
        // data.roleData.level = roleData.level;
    }
}
