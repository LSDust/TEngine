using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public static partial class PersistenceSystem
    {
        public static SaveData SaveData = SaveDataExtensions.CreateNew();
    }
}
