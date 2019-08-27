# Getting Started
The source code for the UI Recorder tool is available on the WinAppDriver repo [here](https://github.com/Microsoft/WinAppDriver/tree/master/Tools). 

# Using UI Recorder

The UI Recorder tool aims to provide an intuitive, and simplistic, user interface that is divided into two panels, as seen below:
<p align="left"><img src="https://github.com/hassanuz/Sandbox/blob/master/snippets/UI%20Recorder/uirecorder_1.PNG?raw=true" width="700" align="middle"></p>

**UI Recorder** tracks both keyboard and mouse interactions against an application interface—representing a **UI action**. When **Recording** is active, both the top and bottom panels are dynamically updated with varying UI element information every time a new UI action takes place. The **Top Panel** shows the generated XPath query of the currently selected UI element, and the **Bottom Panel** shows the **raw XML information** for the same element. You can navigate to the **C# Code tab** on the bottom panel to view generated C# code of the recorded action which you can use on a WinAppDriver test. 
