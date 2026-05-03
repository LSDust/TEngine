## 一、修复
1. 修改包引用
   - 添加TMP
   - 删除accessibility

2. 版本库忽略UserSettings目录
   - #Supplement
   - [Uu]serSettings/
   - .vscode/
   - *.slnx

3. 完善.editorconfig
   - utf-8
   - 4空格的Tab
  
4. 初次打开项目
   - 编辑器版本切换
   - 过时API打开时自动更新

5. 更新插件
6. 修改根命名空间

## 二、Input System
1. 导入Input System
   - 设置中停用旧输入系统
   <!-- - UI的EventSystem点一下(顺便加个平行光) -->
   - 将InputModule目录拖拽到Module下
   - 设定InputSystem_Actions

2. InputSystem_Actions类使用
   - 创建IInputModule和InputModule
   - 在GameModule中添加IInputModule

## 三、导入URP
1. 导入URP包重启
2. 全局材质修复一次

## 四、原始场景修改
1. 替换Main场景
2. UI预制体修改
   - 将相机转换为URP相机
   - EventSystem调整为InputSystem
   - Canvas修改
3. 导入TMP
4. 添加UI_Environment场景
## 五、新建基础场景和场景跳转
1. 完善main场景
   - 导入中文字体
   - 添加UI预制体
   - 给UI相机添加组件
2. 添加场景Loading
   - 扩展SceneModule,添加带进度条的场景加载方式
3. 流程梳理
   - StartGame -> Menu -> ReadDataLoadScene
## 六、序列化和持久化优化
1. 序列化:导入Newtonsoft.Json
2. 持久化:
   - 使用https://github.com/immortal5205/UnityArchiveSystem.git 开源项目
   - 视频案例 www.bilibili.com/video/BV1mw9gBgEng





