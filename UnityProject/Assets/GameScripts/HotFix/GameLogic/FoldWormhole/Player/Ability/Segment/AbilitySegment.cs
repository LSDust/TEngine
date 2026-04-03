using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Cysharp.Threading.Tasks;

namespace GameLogic
{
    /// <summary>
    /// 技能行为段基类
    /// </summary>
    public abstract class AbilitySegment
    {
        public string SegmentId { get; set; }
        public string Type { get; set; }
        public int Level { get; set; }
        public AbilityTemplate Template { get; set; }
        
        // ========== 生命周期钩子（子类可选实现） ==========
        
        /// <summary>
        /// Segment 进入时调用（在 Execute 之前）
        /// </summary>
        public virtual void OnEnter(AbilityContext context)
        {
            if (Template != null)
            {
                Level = Template.Level;
            }
            Debug.Log($"[Segment] {SegmentId} ({Type}) OnEnter, Level: {Level}");
        }
        
        public abstract UniTask<SegmentResult> Execute(AbilityContext context);
        
        /// <summary>
        /// Segment 退出时调用（在 Execute 之后）
        /// </summary>
        public virtual void OnExit(AbilityContext context)
        {
            Debug.Log($"[Segment] {SegmentId} ({Type}) OnExit");
        }
        
        /// <summary>
        /// Segment 被中断时调用
        /// </summary>
        public virtual void Interrupt(AbilityContext context)
        {
            Debug.Log($"[Segment] {SegmentId} ({Type}) Interrupt");
        }
    }
    
    /// <summary>
    /// Segment 执行结果
    /// </summary>
    public class SegmentResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;
        
        /// <summary>
        /// 输出数据（传递给下一个 Segment）
        /// </summary>
        public JObject Data { get; set; } = new JObject();
        
        /// <summary>
        /// 失败原因（Success=false 时填写）
        /// </summary>
        public string Error { get; set; }
        
        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static SegmentResult Ok(JObject data = null)
        {
            return new SegmentResult 
            { 
                Success = true, 
                Data = data ?? new JObject() 
            };
        }
        
        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static SegmentResult Fail(string error)
        {
            return new SegmentResult 
            { 
                Success = false, 
                Error = error 
            };
        }
        
        public override string ToString()
        {
            return $"SegmentResult(Success:{Success}, Error:{Error})";
        }
    }
}
