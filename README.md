# RhinoPythonForVS

RhinoPythonForVS is a plugin to allow you to code Rhino python script in Visual Studio, the world's best IDE!

However, if you prefer VS Code, there is a **[RhinoPythonForVSCode](https://github.com/ccc159/RhinoPythonForVscode)** plugin.

It is a **[DesignToProduction](http://designtoproduction.com/)** open source project, programmed initially for internal use.


## Features

**Super powerful IntelliSense** in realtime. **Much faster** and responsive than the 
[VS Code Demo](https://www.youtube.com/watch?v=QbmnKFIKBYs&feature=youtu.be) 

## Requirements

This is the client side of RhinoPython editor. To bridge it to Rhino you need a server to listen to Visual Studio, which is another plugin called [CodeListener](https://github.com/ccc159/CodeListener).

## Installation


+ Make sure you have **[IronPyton 2.7.5](https://github.com/IronLanguages/main/releases/tag/ipy-2.7.5)** installed. Note: The latest version 2.7.9 is buggy, tested by author.
+ Make sure you have **Visual Studio 2015** installed. Note: VS2017, VS2019 are not tested yet.
+ Make sure you have **[PTVS(Python Tools for Visual Studio)](https://docs.microsoft.com/en-us/visualstudio/python/installing-python-support-in-visual-studio?view=vs-2017#visual-studio-2015)** installed.
+ Make sure you have **CodeListener** plugin for Rhino in [CodeListener latest release](https://github.com/ccc159/CodeListener/releases) installed. Note: You have to download the **CodeListener.rhi** file for the first time installation.
+ Install latest **[RhinoPythonForVisualStudio.vsix](https://github.com/ccc159/RhinoPythonForVS/releases)**

## Preparation
+ Copy desired **.dll** libraries under `C:\Program Files (x86)\IronPython 2.7\DLLs` folder. *e.g.* Copy **RhinoCommon.dll** from `C:\Program Files\Rhinoceros 5 (64-bit)\System\` and **System.Drawings.dll** from `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\XXX`
+ Copy **all files** from `C:\Users\jch\AppData\Roaming\McNeel\Rhinoceros\5.0\Plug-ins\IronPython (814d908a-e25c-493d-97e9-ee3861957f49)\settings\lib` into `C:\Program Files (x86)\IronPython 2.7\Lib\` folder. Note: the **site-packages** folder seems can't invoke intellisense, tested only by author.
+ Start Visual Studio, click **Tools -> Python Tools -> Python Environments**, from the *Python Environments* window on the right side, select **IronPython 2.7**, switch from **Overview** to **IntelliSense**, click **Refresh DB**


## Usage

+ Start Rhino, type command `CodeListener`
+ Start Visual Studio, create a new **IronPython Application** project.
+ Right click on **Search Paths** In **Solution Explorer** and add folder to the path if you want to add extra library.
+ When finished coding, click **RhinoPython -> SendCode/SendCodeWithoutReset** or **Shift + F2** to run code in Rhino.
+ Feel free if you want to [custom you own shortcut for the commands](https://docs.microsoft.com/en-us/visualstudio/ide/identifying-and-customizing-keyboard-shortcuts-in-visual-studio?view=vs-2017).




## Known Issues

- The debugger has not implemented yet.
- It only supports one Rhino instance at a time. If you want to switch Rhino instance, either close the former Rhino instance or command `StopCodeListener` on the former one.
