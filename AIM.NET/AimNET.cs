using org.aim.api.measurement.collector;
using org.aim.artifacts.records;
using org.aim.mainagent.csharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace AIM_NET
{
    /// <summary>
    /// Utility class to access AIM and its DataCollector.
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

        /// <summary>
        /// Returns the DataCollector specified by AIM.
        /// </summary>
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
            DotNetAgent.start("C:\\aim.config");
        }
    }
}
