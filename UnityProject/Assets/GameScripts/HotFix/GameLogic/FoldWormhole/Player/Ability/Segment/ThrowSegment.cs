using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GameLogic
{
    public class ThrowSegment : AbilitySegment
    {
        public struct ThrowSegmentData
        {
            public string ProjectilePrefabPath { get; set; }
            public float Speed { get; set; }
            public float Damage { get; set; }
            public float Range { get; set; }
        }

        public ThrowSegmentData[] LevelData { get; set; }

        public override void OnEnter(AbilityContext context)
        {
            base.OnEnter(context);
            
            var data = GetCurrentLevelData();
            Debug.Log($"[ThrowSegment] Enter - Level: {Level}, Projectile: {data.ProjectilePrefabPath}, Speed: {data.Speed}, Damage: {data.Damage}, Range: {data.Range}");
        }

        public override async UniTask<SegmentResult> Execute(AbilityContext context)
        {
            if (context.Caster == null)
            {
                return SegmentResult.Fail("Caster is null");
            }

            var data = GetCurrentLevelData();

            Vector3 spawnPos = context.Caster.transform.position + context.Caster.transform.forward * 1f + context.Caster.transform.up * 1.5f;
            Quaternion spawnRot = context.Caster.transform.rotation;

            Debug.Log($"[ThrowSegment] Spawning projectile at {spawnPos}");
            Debug.Log($"[ThrowSegment] Projectile will travel at {data.Speed} m/s, damage: {data.Damage}, range: {data.Range}");

            await UniTask.Delay(100);

            JObject resultData = new JObject();
            resultData["spawnPos"] = new JObject
            {
                { "x", spawnPos.x },
                { "y", spawnPos.y },
                { "z", spawnPos.z }
            };
            resultData["direction"] = new JObject
            {
                { "x", context.Caster.transform.forward.x },
                { "y", context.Caster.transform.forward.y },
                { "z", context.Caster.transform.forward.z }
            };
            resultData["damage"] = data.Damage;

            return SegmentResult.Ok(resultData);
        }

        private ThrowSegmentData GetCurrentLevelData()
        {
            if (LevelData == null || LevelData.Length == 0)
            {
                return default;
            }
            
            int index = Mathf.Clamp(Level, 0, LevelData.Length - 1);
            return LevelData[index];
        }

        public override void OnExit(AbilityContext context)
        {
            base.OnExit(context);
            Debug.Log("[ThrowSegment] Exit");
        }

        public override void Interrupt(AbilityContext context)
        {
            base.Interrupt(context);
            Debug.Log("[ThrowSegment] Interrupt");
        }
    }
}
