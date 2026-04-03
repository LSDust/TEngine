using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;

namespace GameLogic
{
    /// <summary>
    /// 技能上下文 - 在技能生命周期中传递数据
    /// </summary>
    public class AbilityContext
    {
        // ========== 核心字段 ==========
        
        /// <summary>
        /// 施法者 GameObject
        /// </summary>
        public GameObject Caster { get; set; }
        
        /// <summary>
        /// 施法位置（世界坐标）
        /// </summary>
        public Vector3 CastPosition { get; set; }
        
        /// <summary>
        /// 技能实例唯一 ID（用于追踪和调试）
        /// </summary>
        public string AbilityInstanceId { get; set; }
        
        // ========== 生命周期相关 ==========
        
        /// <summary>
        /// 当前技能状态
        /// </summary>
        public AbilityState CurrentState { get; set; } = AbilityState.Idle;
        
        // ========== 数据传递（Segment 间共享） ==========
        
        /// <summary>
        /// Segment 间共享数据（使用 JObject 便于序列化）
        /// </summary>
        public JObject SharedData { get; set; } = new JObject();
        
        // ========== 回调 ==========
        
        /// <summary>
        /// 状态变更回调
        /// </summary>
        public Action<AbilityState, AbilityState> OnStateChange { get; set; }
        
        /// <summary>
        /// Segment 完成回调
        /// </summary>
        public Action<string, bool> OnSegmentComplete { get; set; }
        
        // ========== 调试支持 ==========
        
        public override string ToString()
        {
            return $"AbilityContext(Instance:{AbilityInstanceId}, State:{CurrentState})";
        }
    }
    
    /// <summary>
    /// 技能生命周期状态
    /// </summary>
    public enum AbilityState
    {
        Idle,       // 空闲（可触发）
        Prepare,    // 准备阶段（前摇/读条）
        Execute,    // 执行阶段（Segment 运行）
        Finish,     // 结束（可清理）
        Interrupted // 被中断
    }
}
