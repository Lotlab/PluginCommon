# ACT Plugin Commmon Library

A library for writing Advanced Combat Tracker plugin.

This library now contains:

- PluginCommon
  - ClassProxy: A proxy class using reflection for avoiding reference assembly.
  - SimpleLogger: A simple logger.
- PluginCommon.WPF
  - PropertyNotifier: Property notifier for MVVM
  - SimpleLoggerSync: Use this for UI logger display
- PluginCommon.FFXIV
  - ACTPluginProxy: A wrapper class for FFXIV ACT Plugin. You could use this without reference FFXIV_ACT_Plugin.Common dll.
  - NetworkParser: An IPC parser for FFXIV network packet.
- PluginCommon.Overlay
  - EventSourceBase: A wrapper class for OverlayPlugin.EventSourceBase

We recommand you use ilmerge to pack this assembly into your project.

# ACT 插件开发公共库

此仓库包含了一些开发ACT插件时常用的方法，包括

- PluginCommon
  - ClassProxy: 使用反射实现的类代理，可以在不引用其他程序集的情况下调用方法
  - SimpleLogger: 简单的日志记录器
- PluginCommon.WPF
  - PropertyNotifier: MVVM使用的属性变更发射器
  - SimpleLoggerSync: 同步版本的事件记录器，UI上使用这个。
- PluginCommon.FFXIV
  - ACTPluginProxy: FFXIV 解析插件代理类。使用这个类可以在不引用 FFXIV_ACT_Plugin.Common.dll 的情况下对插件操作
  - NetworkParser: FFXIV 网络包解析器，内置了一些包。
- PluginCommon.Overlay
  - EventSourceBase: OverlayPlugin.EventSourceBase 的替代类，可以在不引用 OverlayPlugin.Core 的情况下编写悬浮窗。

如果你觉得还有什么需要添加的东西，欢迎开 Pull Request
