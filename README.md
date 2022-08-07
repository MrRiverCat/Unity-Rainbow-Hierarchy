# Unity Rainbow Hierarchy  彩虹Hierarchy
Description EN：A personal implementation that mimics the Unity Assets Store's famous Rainbow plugin.  
Description CN：仿照 Unity Assets Store 中最热门的 Rainbow 插件，以个人角度思考与实现 Hierarchy 面板中的彩虹效果。  
&emsp;

## 实现思路解析与分享
You can learn about this feature implementation through the following links.  
你可以通过以下链接配合理解实现方式 ——》 URL：https://blog.csdn.net/qq_51026638/article/details/125204689

![image](https://user-images.githubusercontent.com/48646973/179403347-3bba1a3a-ac07-4481-8d25-ddcd4335f61a.png)



## 使用指导
### 01 部署 `RinbowHierarchy.cs` 与 `RainbowHierarachyConfig.cs` 脚本存放至资产路径 Editor 目录下。
![image](https://user-images.githubusercontent.com/48646973/179143289-6488a1e1-d38d-4bcf-b9f9-c1a00947c79a.png)

### 02 创建新场景对象 或 使用已打开场景对象  

### 03 选择对象，添加 Rainbow Hierarchy 标记
![image](https://user-images.githubusercontent.com/48646973/179143425-653cf6b5-76a4-4556-bc4c-71aa7dd8b60a.png)

### 04 使用 ScriptableObject 调整数据状态
![image](https://user-images.githubusercontent.com/48646973/179143204-e8bd2385-48a8-4c0f-a73a-25dbf63599b2.png)  


## 已知问题
1. 在进行工程打包过程中，因为该 Plugin 导致打包过程出现未知的错误异常。
2. 初次应用于新场景中，出现空引用报错，但仅需点击后即可自动创建SO文件。


## 相关说明
 01. 本插件基于 Unity Editor 模式下运行。不支持 Unity Runtime 模式下下运行。  
 02. Rainbow Hierarchy 仅从视图上强调 GameObject 对象的重要性，并不会对项目整体有任何影响。  
 03. 所有的彩虹数据调整，仅支持路径下伴随创建的 `ScriptableObject` 进行手动修改。暂未更新更加便捷的内容更变方式，后续时间允许，会考虑优化这部分功能。  
 04. 如有该插件功能问题，请留言。  
