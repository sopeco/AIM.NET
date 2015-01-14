using org.aim.api.measurement.collector;
using org.aim.artifacts.records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AIM.NET
{
    /// <summary>
    /// 
    /// </summary>
    public class AimNET
    {
        public static IDataCollector DataCollector
        {
            get;
            private set;
        }

        public void Init()
        {
            DataCollector = AbstractDataSource.getDefaultDataSource();
        }

        public void MakeTestRecord()
        {
            ResponseTimeRecord rtr = new ResponseTimeRecord();

            rtr.setCallId(1L);
            rtr.setOperation("method");
            rtr.setResponseTime(10L);
            rtr.setTimeStamp((long)(new DateTime() - new DateTime(1970, 1, 1)).TotalMilliseconds);

            DataCollector.newRecord(rtr);
        }
    }
}
