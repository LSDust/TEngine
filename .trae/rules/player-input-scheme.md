---
alwaysApply: false
description: 编写涉及玩家输入的代码时
---
# Input System 规范

## 基本原则

- 该 Unity 项目**只使用 Input System**，禁止使用旧的 Input Manager
- 所有输入相关功能必须通过 Unity Input System 实现

## 输入方案

- 使用 Unity Input System 的 `Input System Package`
- 项目使用 `InputSystem_Actions.inputactions` 资产，包含 Player 和 UI 两个 Action Map

## 代码规范

- 通过 `GameModule.Input` 访问输入系统
- 使用 `IInputModule.IPlayerActions` 接口处理玩家输入
- 使用 `IInputModule.IUIActions` 接口处理 UI 输入
- 使用 `AddCallbacks()` / `RemoveCallbacks()` 注册回调
- 详细使用方式参见 `Books/Folded Wormhole/输入方案/输入方案架构文档.md`

## 注意事项

- 确保 Player Settings 中已启用 "Active Input Handling" 为 "Input System Package (New)"
- 所有输入相关代码不得引入 `UnityEngine.Input` 命名空间（旧输入系统）
