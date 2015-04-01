using org.aim.artifacts.records;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace AIM_NET.Aspects
{
    /// <summary>
    /// Aspect to measure the response times of methods.
    /// </summary>
    [PSerializable]
    public class ResponseTimeAspect : AbstractMethodInterceptionAspect
    {                
        /// <summary>
        /// This method is invoked when the instrumented target method is invoked.
        /// </summary>
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            ResponseTimeRecord record = new ResponseTimeRecord();
            
            record.setTimeStamp((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds);
            record.setOperation(FullMethodName);
            record.setProcessId("" + Process.GetCurrentProcess().Id);

            long timeStart = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            base.OnInvoke(args);
            long timeEnd = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;

            record.setTimeStamp(timeEnd);
            record.setResponseTime(timeEnd - timeStart);

            AIMnet.Instance.DataCollector.newRecord(record);
        }
    }
}
