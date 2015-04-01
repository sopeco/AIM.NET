# AIM.NET

## SetUp

PostSharp is required and has to be added to the project

jnBridge is requried and has to be added to the project. Additionally, the following classes are required and have to be provided by a jnBridge proxy DLL.
A compiled jnBridge proxy (.NET45) is included in the repository.

Specify aim.config in AIMnet class. (This is a hardcoded value at the moment)

###Dependencies
JNBShare.dll

### Files in project
rlm932_x64.dll
rlm932_x86.dll
App.config

## More

### Required Java classes
org.aim.api.measurement.**AbstractRecord**
org.aim.api.measurement.collector.**AbstractDataSource**
org.aim.api.measurement.collector.**IDataCollector**
org.aim.artifacts.records.**ResponseTimeRecord**
org.aim.artifacts.records.**SQLQueryRecord**
org.aim.mainagent.csharp.**Starter**