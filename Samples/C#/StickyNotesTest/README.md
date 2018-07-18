# StickyNotesTest

StickyNotesTest is a sample test project that demonstrates how to perform pen interactions through the actions API using the Windows 10 built-in **Sticky Notes** application.

This test project highlights some common pen interactions below.
- Drawing using simple pen strokes
- Drawing strokes with additional parameters such as pressure, twist, and tilt angles
- Erasing previously drawn stroke by using eraser button


## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Microsoft Visual Studio 2017 or later


## Getting Started

1. [Run](../../../README.md#installing-and-running-windows-application-driver) `WinAppDriver.exe` on the test device
1. Open `StickyNotesTest.sln` in Visual Studio
2. Select **Test** > **Windows** > **Test Explorer**
3. Select **Run All** on the test pane or through menu **Test** > **Run** > **All Tests**

> Once the project is successfully built, you can use the **TestExplorer** to pick and choose the test scenario(s) to run


## Tutorial

### Including proper appium-dotnet-driver NuGet package

Pen input support in Windows Application Driver is accessed using W3C WebDriver Actions API. Currently this feature is only supported
through a temporary `WinAppDriver.Preview.Appium.WebDriver` instead of the official `Appium.WebDriver` NuGet package. This sample test
is using this temporary NuGet package as shown in [packages.config](./packages.config).

To make use all the pen features through the library in the NuGet package above, use the `OpenQA.Selenium.Appium.Interactions.PointerInputDevice`
instead of the `OpenQA.Selenium.Interactions.PointerInputDevice`.

### Drawing a pen stroke

A pen stroke is a collection of **PointerDown**, **PointerMove**, and **PointerUp** in an **ActionSequence** as shown below.
```c#
PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
ActionSequence sequence = new ActionSequence(penDevice, 0);

// Draw line AB from point A (0, 0) to B (10, 10)
sequence.AddAction(penDevice.CreatePointerMove(CoordinateOrigin.Viewport, 0, 0, TimeSpan.Zero));
sequence.AddAction(penDevice.CreatePointerDown(PointerButton.PenContact));
sequence.AddAction(penDevice.CreatePointerMove(CoordinateOrigin.Viewport, 10, 10, TimeSpan.Zero));
sequence.AddAction(penDevice.CreatePointerUp(PointerButton.PenContact));

session.PerformActions(new List<ActionSequence> { sequence });
```

### Drawing with additional pen attributes

`OpenQA.Selenium.Appium.Interactions.PenInfo` class contains all the supported attributes for pen input such as **Pressure**, **Twist**,
**TiltX**, and **TiltY**. An instance containing any of the attribute can be passed into any pointer event creation. For example:
```c#
PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
ActionSequence sequence = new ActionSequence(penDevice, 0);
PenInfo penExtraAttributes = new PenInfo { TiltX = 45, TiltY = 45, Twist = 45 };

// Draw line AB from point A (0, 0) to B (10, 10) with attributes defined in penExtraAttributes
sequence.AddAction(penDevice.CreatePointerDown(PointerButton.PenContact, penExtraAttributes));
sequence.AddAction(penDevice.CreatePointerMove(CoordinateOrigin.Pointer, 10, 10, TimeSpan.Zero, new PenInfo { Pressure = 1f }));
sequence.AddAction(penDevice.CreatePointerUp(PointerButton.PenContact));

session.PerformActions(new List<ActionSequence> { sequence });
```

### Erasing a pen stroke

When inking on a canvas, a pen stroke can be erased by drawing another stroke on top of it while depressing the **Eraser** button.
Notice the use of `PointerButton.Eraser` instead of `PointerButton.PenContact`. For example:
```c#
PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
ActionSequence sequence = new ActionSequence(penDevice, 0);

// Erase line AB by pressing PenEraser (Pen tail end/eraser button) on the line AB
sequence.AddAction(penDevice.CreatePointerMove(CoordinateOrigin.Viewport, 0, 0, TimeSpan.Zero));
sequence.AddAction(penDevice.CreatePointerDown(PointerButton.PenEraser));
sequence.AddAction(penDevice.CreatePointerMove(CoordinateOrigin.Viewport, 10, 10, TimeSpan.Zero));
sequence.AddAction(penDevice.CreatePointerUp(PointerButton.PenEraser));

session.PerformActions(new List<ActionSequence> { sequence });
```

### Specifying pointer move coordinates

Windows Application Driver supports all W3C WebDriver pointer action **origins** as follows:
1. **Viewport** - **X** and **Y** are relative to the application session window
2. **Pointer** - **X** and **Y** are relative to the current **X** and **Y** position
3. **Element** - **X** and **Y** are relative to the center point of the element


## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and conciseness:
1. Group test methods based on scenario such as Drawing
2. Maintain original state after test runs and keep clean state in between tests when possible
3. Only add tests that provide additional value to the sample
