# AIM.NET

## SetUp

PostSharp is required and has to be added to the project

JNBridge is requried and has to be added to the project. Additionally, the following classes are required and have to be provided by a JNBridge proxy DLL.

###Dependencies
JNBShare.dll

### Files in project
rlm932_x64.dll
rlm932_x86.dll
App.config

## More

### Required Java classes
* org.aim.api.measurement.**AbstractRecord**
* org.aim.api.measurement.collector.**AbstractDataSource**
* org.aim.api.measurement.collector.**IDataCollector**
* org.aim.artifacts.records.**ResponseTimeRecord**
* org.aim.artifacts.records.**SQLQueryRecord**
* org.aim.mainagent.csharp.**DotNetAgent**
