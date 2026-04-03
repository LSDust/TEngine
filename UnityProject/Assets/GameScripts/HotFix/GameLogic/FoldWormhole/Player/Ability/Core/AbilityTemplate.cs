using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace GameLogic
{
    public partial class AbilityTemplate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AbilitySegment> Segments { get; set; }
        public int Level { get; set; }
        
        public void OnDeserialized()
        {
            if (Segments != null)
            {
                foreach (var segment in Segments)
                {
                    segment.Template = this;
                }
            }
        }
        
        public override string ToString()
        {
            return $"AbilityTemplate(Id:{Id}, Name:{Name},Description:{Description}, Segments:{string.Join(",",Segments)})";
        }

        public async UniTask<SegmentResult> ExecuteTemplate(AbilityContext context)
        {
            foreach (var segment in this.Segments)
            {
                if (context.CurrentState == AbilityState.Interrupted)
                {
                    // Debug.Log($"[CasterAbilityComponent] Ability interrupted: {abilityId}");
                    return SegmentResult.Fail("Ability interrupted");
                }

                // 调用Segment的OnEnter方法
                segment.OnEnter(context);

                // 执行Segment
                SegmentResult result = null;
                try
                {
                    result = await segment.Execute(context);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[CasterAbilityComponent] Error executing segment: {e.Message}");
                    result = SegmentResult.Fail($"Error executing segment: {e.Message}");
                }

                // 调用Segment的OnExit方法
                segment.OnExit(context);

                // 触发Segment完成回调
                context.OnSegmentComplete?.Invoke(segment.SegmentId, result.Success);

                // 如果Segment执行失败，技能释放失败
                if (!result.Success)
                {
                    Debug.LogError($"[CasterAbilityComponent] Segment failed: {result.Error}");
                    // UpdateAbilityState(abilityId, AbilityState.Finish);
                    context.CurrentState = AbilityState.Finish;
                    context.OnStateChange?.Invoke(AbilityState.Execute, AbilityState.Finish);
                    return result;
                }

                // 短暂延迟，使技能执行更流畅
                await UniTask.Delay(50);
            }
            return SegmentResult.Ok();
        }
    }
}
