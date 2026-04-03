using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GameLogic
{
    public class MoveSegment : AbilitySegment
    {
        public struct MoveSegmentData
        {
            public int Distance { get; set; }
            public bool IsTeleport { get; set; }
        }

        public MoveSegmentData[] LevelData { get; set; }

        public override void OnEnter(AbilityContext context)
        {
            base.OnEnter(context);
            
            var data = GetCurrentLevelData();
            Debug.Log($"[MoveSegment] Enter - Level: {Level}, Distance: {data.Distance}, IsTeleport: {data.IsTeleport}");
        }

        public override async UniTask<SegmentResult> Execute(AbilityContext context)
        {
            if (context.Caster == null)
            {
                return SegmentResult.Fail("Caster is null");
            }

            var data = GetCurrentLevelData();
            if (data.Distance == 0)
            {
                return SegmentResult.Fail("MoveSegment has no valid LevelData");
            }

            Vector3 startPos = context.Caster.transform.position;
            Vector3 targetPos = startPos + context.Caster.transform.forward * data.Distance;

            if (data.IsTeleport)
            {
                context.Caster.transform.position = targetPos;
                Debug.Log($"[MoveSegment] Teleported to {targetPos}");
            }
            else
            {
                float duration = 0.5f;
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    float t = elapsed / duration;
                    context.Caster.transform.position = Vector3.Lerp(startPos, targetPos, t);
                    elapsed += Time.deltaTime;
                    await UniTask.Yield();
                }
                context.Caster.transform.position = targetPos;
                Debug.Log($"[MoveSegment] Moved to {targetPos}");
            }

            JObject resultData = new JObject();
            resultData["startPos"] = new JObject
            {
                { "x", startPos.x },
                { "y", startPos.y },
                { "z", startPos.z }
            };
            resultData["targetPos"] = new JObject
            {
                { "x", targetPos.x },
                { "y", targetPos.y },
                { "z", targetPos.z }
            };

            return SegmentResult.Ok(resultData);
        }

        private MoveSegmentData GetCurrentLevelData()
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
            Debug.Log("[MoveSegment] Exit");
        }

        public override void Interrupt(AbilityContext context)
        {
            base.Interrupt(context);
            Debug.Log("[MoveSegment] Interrupt");
        }
    }
}
