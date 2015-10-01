TeamCity BuildMonitor
===================

A simple build monitor for TeamCity using ASP.NET MVC 4 with the following features:

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

Download the repository and compile it on order to download all required NuGet packages. If you dont have automatic NuGet package restore enabled in Visual Studio then it will have to be enabled.

Open Web.config and enter your TeamCity server information into the appSettings labeled teamcity_username, teamcity_password and teamcity_api_url.

In the constructor of HomeController.cs, you can switch between using DefaultBuildMonitorModelHandler (shows all jobs in TeamCity automatically) or the CustomBuildMonitorModelHandler which allows you to customize what to display. You can customize your personal view by editing the file App_Data/Settings.xml.