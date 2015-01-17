using org.aim.api.measurement.collector;
using org.aim.artifacts.records;
using org.aim.mainagent.csharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIM_NET
{
    /// <summary>
    /// 
    /// </summary>
    public class AIMnet
    {
        private static AIMnet instance;
        public static AIMnet Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AIMnet();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        private IDataCollector dataCollector;

        public IDataCollector DataCollector
        {
            get
            {
                if (dataCollector == null)
                {
                    DataCollector = AbstractDataSource.getDefaultDataSource();
                }
                return dataCollector;
            }
            private set
            {
                dataCollector = value;
            }
        }

        private AIMnet()
        {
            Starter.startAIM();
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
