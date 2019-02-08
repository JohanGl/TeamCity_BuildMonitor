TeamCity BuildMonitor
===================

A simple build monitor for TeamCity using ASP.NET Core 2.2 with the following features:

- Build configuration name
- Active branch
- Triggered-by user name
- Running build completion percentage
- Queued builds
- Automatic refresh with a 20 seconds interval
- Groups (shown as backend, frontend and tests in the screenshot below)
- Displays all build configurations automatically (default)
- Can be customized to display custom groups and build configurations

![](https://raw.githubusercontent.com/JohanGl/TeamCity_BuildMonitor/master/BuildMonitor.png)

----------

Installation
-------------

Make sure that you have ASP.NET Core 2.2 installed.

Open appsettings.json and enter your TeamCity server information under the TeamCity node. UserName and Password will be automatically read/overridden from a user secrets file if available.

In the constructor of the class IndexModel, you can switch between using DefaultBuildMonitorModelHandler (shows all jobs in TeamCity automatically) or the CustomBuildMonitorModelHandler which allows you to customize what to display. You can customize your personal view by editing the file App_Data/Settings.xml.