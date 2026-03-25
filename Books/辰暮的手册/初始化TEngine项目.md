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

## 四、 替换Main场景
   1. 两个预制体修改
   2. 将相机转换为URP相机





