# Ability 技能模块架构文档

## 1. 模块概述

Ability 模块是项目中实现游戏技能系统的核心组件，采用**数据驱动 + 行为段(Segment)组合**的设计模式。该模块允许通过 JSON 配置文件定义技能模板，并支持通过组合不同的行为段来构建复杂的技能效果。

### 核心特性

- **数据驱动**：技能模板通过 JSON 配置定义，便于策划调整参数
- **行为段组合**：技能由多个 Segment 组成，每个 Segment 负责特定行为
- **异步执行**：基于 UniTask 实现异步技能释放流程
- **状态管理**：完整的状态机管理技能生命周期

---

## 2. 目录结构

```
Assets/GameScripts/HotFix/GameLogic/MyLogic/Player/Ability/
├── Core/                         # 核心组件
│   ├── AbilityContext.cs           # 技能上下文
│   ├── AbilitySegment.cs           # 行为段基类
│   ├── AbilityTemplate.cs          # 技能模板
│   └── CasterAbilityComponent.cs   # 施法者技能组件
├── Segment/                      # 具体行为段实现
│   ├── MoveSegment.cs              # 位移行为段
│   └── ThrowSegment.cs             # 投射物行为段
└── Tool/                         # 工具类
    └── KnownTypesBinder.cs         # JSON反序列化类型绑定器
```

---

## 3. 核心类设计

### 3.1 AbilityContext(技能上下文)

技能上下文在技能整个生命周期中传递数据，是 Segment 之间通信的桥梁。

```csharp
public class AbilityContext
{
    // 核心字段
    public GameObject Caster { get; set; }           // 施法者
    public Vector3 CastPosition { get; set; }         // 施法位置
    public string AbilityInstanceId { get; set; }     // 技能实例ID
    
    // 生命周期状态
    public AbilityState CurrentState { get; set; }    // 当前状态
    
    // 数据传递
    public JObject SharedData { get; set; }           // Segment间共享数据
    
    // 回调
    public Action<AbilityState, AbilityState> OnStateChange { get; set; }
    public Action<string, bool> OnSegmentComplete { get; set; }
}
```

#### 技能状态机

```csharp
public enum AbilityState
{
    Idle,        // 空闲(可触发)
    Prepare,     // 准备阶段(前摇/读条)
    Execute,     // 执行阶段(Segment运行)
    Finish,      // 结束(可清理)
    Interrupted  // 被中断
}
```

**状态流转图：**

```
Idle → Prepare → Execute → Finish
                   ↓
              Interrupted
```

---

### 3.2 AbilitySegment(行为段基类)

所有技能行为段的抽象基类，定义了生命周期钩子和执行接口。

```csharp
public abstract class AbilitySegment
{
    public string SegmentId { get; set; }
    public string Type { get; set; }
    
    // 生命周期钩子
    public virtual void OnEnter(AbilityContext context) { }
    public abstract UniTask<SegmentResult> Execute(AbilityContext context);
    public virtual void OnExit(AbilityContext context) { }
    public virtual void Interrupt(AbilityContext context) { }
}
```

#### SegmentResult(执行结果)

```csharp
public class SegmentResult
{
    public bool Success { get; set; }           // 是否成功
    public JObject Data { get; set; }            // 输出数据
    public string Error { get; set; }            // 失败原因
    
    public static SegmentResult Ok(JObject data = null)
    public static SegmentResult Fail(string error)
}
```

---

### 3.3 AbilityTemplate(技能模板)

技能模板定义了技能的静态配置，包含技能 ID、名称、描述和一系列行为段。

```csharp
public class AbilityTemplate
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<AbilitySegment> Segments { get; set; }
    
    public async UniTask<SegmentResult> ExecuteTemplate(AbilityContext context)
}
```

**执行流程：**

1. 遍历所有 Segment
2. 检查技能是否被中断
3. 调用 `OnEnter` 钩子
4. 执行 `Execute` 方法
5. 调用 `OnExit` 钩子
6. 触发 `OnSegmentComplete` 回调
7. 任何 Segment 失败则技能释放失败

---

### 3.4 CasterAbilityComponent(施法者组件)

挂载在角色身上的技能管理组件，负责技能的加载、释放和状态管理。

```csharp
public class CasterAbilityComponent : SerializedMonoBehaviour
{
    public static List<AbilityTemplate> Templates { get; }  // 技能模板库
    
    private Dictionary<string, AbilityState> _abilityStates;  // 技能状态
    
    public async UniTask<SegmentResult> CastAbility(string abilityId, AbilityContext context)
    public AbilityState GetAbilityState(string abilityId)
    public bool CanCastAbility(string abilityId)
    
    public static async UniTaskVoid LoadTemplatesFromJson()
}
```

**技能释放流程：**

```
1. 检查技能是否存在
2. 检查 AbilityContext
3. 设置施法者
4. 获取技能模板
5. 更新状态: Idle → Prepare
6. 延迟 100ms(模拟前摇)
7. 更新状态: Prepare → Execute
8. 执行模板(所有Segment)
9. 更新状态: Execute → Finish
10. 返回结果
```

---

## 4. 具体 Segment 实现

### 4.1 MoveSegment(位移段)

负责角色的位移效果，支持瞬移和平滑移动两种模式。

```csharp
public class MoveSegment : AbilitySegment
{
    public float Distance { get; set; } = 5f;    // 位移距离
    public float Duration { get; set; } = 0.5f;  // 移动耗时
    public bool IsTeleport { get; set; } = false; // 是否瞬移
}
```

**位移逻辑：**

- **瞬移模式**：直接设置目标位置
- **平滑移动**：使用 `Vector3.Lerp` 插值移动

**输出数据：**

```json
{
    "startPos": { "x": 0, "y": 0, "z": 0 },
    "targetPos": { "x": 0, "y": 0, "z": 5 }
}
```

---

### 4.2 ThrowSegment(投射物段)

负责生成投射物(如火球、箭矢)，可扩展为完整的弹道系统。

```csharp
public class ThrowSegment : AbilitySegment
{
    public string ProjectilePrefabPath { get; set; }  // 预制体路径
    public float Speed { get; set; } = 20f;           // 飞行速度
    public float Damage { get; set; } = 20f;          // 伤害值
    public float Range { get; set; } = 20f;           // 射程
}
```

**生成逻辑：**

1. 计算生成位置(角色前方1米 + 上方1.5米)
2. 获取角色朝向作为发射方向
3. 加载预制体(当前为简化实现)
4. 模拟投射物飞行

**输出数据：**

```json
{
    "spawnPos": { "x": 0, "y": 1.5, "z": 1 },
    "direction": { "x": 0, "y": 0, "z": 1 },
    "damage": 20
}
```

---

## 5. 配置文件格式

技能模板通过 JSON 文件定义，存储在 `Assets/AssetRaw/Configs/AbilityTemplates.json`

```json
[
  {
    "Id": "Skill1",
    "Name": "一技能",
    "Description": "移动技能",
    "Segments": [
      {
        "$type": "GameLogic.MoveSegment, GameLogic",
        "Distance": 5.0,
        "Duration": 0.5,
        "IsTeleport": false
      },
      {
        "$type": "GameLogic.ThrowSegment, GameLogic",
        "ProjectilePrefabPath": "Projectiles/Fireball",
        "Speed": 20.0,
        "Damage": 20.0,
        "Range": 20.0
      }
    ]
  }
]
```

**关键点：**

- `$type` 字段指定 Segment 的具体类型
- 使用 `KnownTypesBinder` 进行类型反序列化

---

## 6. JSON 反序列化

### KnownTypesBinder

用于 JSON.NET 的自定义类型绑定器，实现白名单机制确保安全性。

```csharp
public class KnownTypesBinder : ISerializationBinder
{
    public IList<Type> KnownTypes { get; set; } = new List<Type>();
    
    public Type BindToType(string assemblyName, string typeName)
    {
        // 仅允许 KnownTypes 中的类型
    }
}
```

**配置示例：**

```csharp
new JsonSerializerSettings
{
    TypeNameHandling = TypeNameHandling.Auto,
    SerializationBinder = new KnownTypesBinder
    {
        KnownTypes = new List<Type>
        {
            typeof(MoveSegment),
            typeof(ThrowSegment)
        }
    }
}
```

---

## 7. 使用示例

### 加载技能模板

```csharp
await CasterAbilityComponent.LoadTemplatesFromJson();
```

### 释放技能

```csharp
var context = new AbilityContext
{
    AbilityInstanceId = Guid.NewGuid().ToString(),
    CastPosition = transform.position
};

var result = await casterComponent.CastAbility("Skill1", context);
if (result.Success)
{
    Debug.Log("技能释放成功");
}
```

---

## 8. 扩展建议

### 8.1 新增 Segment 类型

1. 创建新的 Segment 类继承 `AbilitySegment`
2. 实现 `Execute` 方法
3. 在 `KnownTypesBinder` 中注册类型
4. 在 JSON 配置中使用 `$type` 引用

### 8.2 常见 Segment 类型

| Segment 类型 | 功能描述 |
|-------------|---------|
| DamageSegment | 造成伤害 |
| HealSegment | 治疗效果 |
| BuffSegment | 添加 Buff |
| AnimationSegment | 播放动画 |
| SoundSegment | 播放音效 |
| EffectSegment | 播放特效 |
| DelaySegment | 延迟等待 |
| ConditionalSegment | 条件分支 |

### 8.3 中断机制

当前支持在 Execute 阶段通过设置 `context.CurrentState = AbilityState.Interrupted` 中断技能执行。

---

## 9. 架构图

```
┌─────────────────────────────────────────────┐
│         CasterAbilityComponent              │
├─────────────────────────────────────────────┤
│  LoadTemplatesFromJson()                    │
├─────────────────────────────────────────────┤
│  CastAbility()                              │
└─────────────────────┬───────────────────────┘
                      │
                      ▼
               ┌──────────────┐
               │   Ability    │
               └──────┬───────┘
                      │
            ┌─────────┴─────────┐
            ▼                   ▼
┌─────────────────────┐ ┌─────────────────────┐
│  AbilityTemplate    │ │  AbilityContext     │
├─────────────────────┤ ├─────────────────────┤
│ Id                  │ │ Caster              │
├─────────────────────┤ ├─────────────────────┤
│ Name                │ │ CastPosition        │
├─────────────────────┤ ├─────────────────────┤
│ Description         │ │ CurrentState        │
├─────────────────────┤ ├─────────────────────┤
│ Segments[]          │ │ SharedData          │
│  ├Segment1          │ ├─────────────────────┤
│  ├Segment2          │ │ OnStateChange       │
│  └Segment3          │ │ OnSegmentComplete   │
└─────────────────────┘ └─────────────────────┘
```

---

## 10. 总结

Ability 模块采用**组合模式**和**数据驱动**的设计，通过 Segment 的灵活组合可以构建出丰富多样的技能效果。该架构具有良好的扩展性，新增技能类型只需：

1. 创建新的 Segment 类
2. 在 JSON 配置中声明
3. 注册到 KnownTypesBinder

这使得技能系统对策划友好，可以不修改代码的情况下调整技能数值和行为。
