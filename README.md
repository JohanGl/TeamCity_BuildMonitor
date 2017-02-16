TeamCity BuildMonitor
===================

A simple build monitor for TeamCity using ASP.NET MVC 4.

Build Monitor Dashboard
-----------------------

This view is optimized to display the results of multiple build configurations with the following features:

- Build configuration name
- Active branch or a pre-selected branch
- Triggered-by user name
- Running build completion percentage
- Queued builds
- Automatic refresh with a 15 seconds interval
- Groups (shown as backend and frontend in the screenshot below)
- Can be customized to display custom groups and build configurations
- Display the history and the last run of the tests on one selected build configuration

![](https://raw.githubusercontent.com/balassy/TeamCity_BuildMonitor/master/BuildMonitor.png)

Automation Monitor Dashboard
----------------------------

This view is optimized to display the results of the tests in multiple build configurations with the following features:

- Build configuration name
- Active branch or a pre-selected branch
- Automatic refresh with a 60 seconds interval
- Groups (shown as backend and frontend in the screenshot below)

![](https://raw.githubusercontent.com/balassy/TeamCity_BuildMonitor/master/AutomationMonitor.png)

Installation
-------------

Download the repository and compile it on order to download all required NuGet packages. If you don't have automatic NuGet package restore enabled in Visual Studio, then it will have to be enabled.

Open `Web.AppSettings.config` and enter your TeamCity server information into the settings labeled `teamcity_username`, `teamcity_password` and `teamcity_api_url`.

In the constructor of `HomeController.cs`, you can switch between using `DefaultBuildMonitorModelHandler` (shows all jobs in TeamCity automatically) or the `CustomBuildMonitorModelHandler` which allows you to customize what to display. You can customize your personal view by editing the file `App_Data/Settings.xml`.

To customize the content of the Automation Monitor dashboard, configure it in the `App_Data/Automation.config` file.