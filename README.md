# 前言

> 如果本项目对你有帮助，请给我`Star`；另外如果你感兴趣的话，可以关注我的博客[Seraphineの小窝](http://blog.helloseraphine.top:8090/)。

这是我的一个不成熟的项目，不论你是毕设也好或者想学习一下别人的`WPF`代码，都可以`fork`本仓库，然后二次修改，**本项目使用的是`MIT`协议**，所以你不需要任何担心。

这个项目名称叫：**林木价值评价系统**，虽然如此，我个人根据认为它的标签应该是：**ArcGIS 地理信息系统 + 森林林木 + 森林经理 综合起来**而形成的软件系统。

# 关于项目

本项目是单机离线的桌面软件，**使用的是`VS 2022`，利用的`WPF`开发技术，基于`MVVM`架构（非教条主义版），`.NET 6`框架**；运行本项目需要具备如下条件：

* **操作系统为`Windows 7`，`windows 10` 或者 `windows 11`等及其以上版本**
* **具备`.NET 6`运行时**

> <del>如果没有`.NET 6`运行时，理论上运行的时候会报错，然后微软给你一个下载链接，下载安装即可</del>
> 我已经打包多个不同版本和类型的安装包，如果你并不了解什么是`.NET6`运行时，则直接下载 **independent** 版本即可。

项目引用了其他`Nuget`包，进行相关功能的支持，如下是感谢支持项目：

* **📌地理信息相关：[Esri.ArcGISRuntime.WPF](https://www.nuget.org/packages/Esri.ArcGISRuntime.WPF)（100.9.0）**
* **📌部分控件支持：[HandyControl](https://github.com/HandyOrg/HandyControl)（3.4.0）**
* **📌图表支持：[LiveChartsCore.SkiaSharpView.WPF](https://github.com/HandyOrg/HandyControl)（2.0.0-beta.701）**
* **📌机器学习运行时：[Microsoft.ML.OnnxRuntime](https://www.nuget.org/packages/Microsoft.ML.OnnxRuntime/1.15.0-alpha)（1.12.0）**
* **📌MVVM工具包：[MvvmLightLibs](https://www.nuget.org/packages/MvvmLightLibs)（5.4.1.1）**
* **📌数据库支持：[System.Data.SQLite](https://www.nuget.org/packages/System.Data.SQLite)（1.0.117）**
* **📌JSON支持：[Newtonsoft.Json](https://www.newtonsoft.com/json)（12.0.3）**

----

软件设计上是比较现代化的，我做了大量的圆角处理，并且在软件系统和相关图标呈现上我也做了动画，加上各种控件，从`UI`美观度上来说，已经非常具有美感且`win 11`化。

从功能上来说，主要的模块部分已经打底完成，需要做的就是向上开发，增加需求即可。

本项目解决方案中，你会看到如下的文件夹结构：

* Data：存储相关数据，目前是数据库存放文件夹，当然你也可以存放其他数据
* Models：MVVM 中的`Models`，请将你的数据类放在这里
* Resources：软件运行所需要的资源，包括但不限于背景图片，字体，图标，视频音频，机器学习模型等
* Services：公共类，里面都是`static`类及其方法，方便其他类的直接调用，包括但是不限于：类型转换类（绑定需要进行的类型转换相关），文件帮助类（负责文件读取，写入相关），数据库帮助类（负责数据库增删查改相关），配置文件帮助类（负责配置文件读写相关）等
* ViewModels：MVVM 中的`ViewModel`，请将你的`ViewModel`放在这里（UI的逻辑处理）
* Views：MVVM 中的`View`，请将你的`View`放在这里（UI）
* ViewPopUp：它已经不是严格的`mvvm`层面，我创建它是因为有写地方需要弹窗来获取相关数据，这个弹窗的`View`我就放在这里面，主要是`Views`文件夹会因为大量的弹窗`view`而变得混乱，我决定给它们单独放一个文件夹

> 本项目将会在6月中旬结束更新，如果你后面需要相关帮助，可以通过访问[我的博客引导页中](https://welcome-1303234197.cos-website.ap-beijing.myqcloud.com/)找到我的联系方式。

# 软件截图

<img src="https://wordpress-serverless-code-ap-beijing-1303234197.cos.ap-beijing.myqcloud.com/PicGo/image-20230525235910237.png" alt="image-20230525235910237" style="zoom:50%;" />

<img src="https://wordpress-serverless-code-ap-beijing-1303234197.cos.ap-beijing.myqcloud.com/PicGo/image-20230525235938513.png" alt="image-20230525235938513" style="zoom:50%;" />

<img src="https://wordpress-serverless-code-ap-beijing-1303234197.cos.ap-beijing.myqcloud.com/PicGo/image-20230526000105543.png" alt="image-20230526000105543" style="zoom:50%;" />

<img src="https://wordpress-serverless-code-ap-beijing-1303234197.cos.ap-beijing.myqcloud.com/PicGo/image-20230516020214588.png" alt="image-20230516020214588" style="zoom:50%;" />

<img src="https://wordpress-serverless-code-ap-beijing-1303234197.cos.ap-beijing.myqcloud.com/PicGo/image-20230516020226939.png" alt="image-20230516020226939" style="zoom:50%;" />

<img src="https://wordpress-serverless-code-ap-beijing-1303234197.cos.ap-beijing.myqcloud.com/PicGo/image-20230516020240251.png" alt="image-20230516020240251" style="zoom:50%;" />

<img src="https://wordpress-serverless-code-ap-beijing-1303234197.cos.ap-beijing.myqcloud.com/PicGo/image-20230526000154773.png" alt="image-20230526000154773" style="zoom:50%;" />

> 注：图示中的图片来源于网络，如有侵权请联系我删除