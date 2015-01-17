using org.aim.artifacts.records;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AIM_NET.Aspects
{
    [Serializable]
    public class ResponseTimeAspect : AbstractMethodAspect
    {
        private class StateInformation
        {
            public long StartTime { set; get; }
            public ResponseTimeRecord Record { set; get; }
        }

        // According to the Postsharp documentation , fields that are only
        // needed at runtime , and are unknown at compile -time , should be
        // marked with the [ NonSerialized ] attribute .
        // [NonSerialized]
        //private static IMonitoringController _ctrlInst;


        private string _componentName;
        private string _method;
        private string _fullMethodName;

        /// <summary >
        /// Compile - time initialization of component name , method name ,
        /// and parameter types of the method to which the current aspect
        /// instance has been applied .
        /// </ summary >
        /// <remarks >
        /// This improves runtime performance , as it avoids use of
        /// <c> System . Reflection </c> as well as string building at
        /// runtime .
        /// </ remarks >
        /// <param name =" method ">
        /// Method to which the current aspect instance has been applied .
        /// </param >
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            _componentName = FormatType(method.DeclaringType.FullName);
            _method = FormatMethodName(method.Name) + FormatParameters(method.GetParameters());
            _fullMethodName = _componentName + "." + _method;
        }

        /// <summary >
        /// Initializes the current aspect at run time .
        /// </ summary >
        /// <param name =" method ">
        /// Method to which the current aspect is applied .
        /// </param >
        public override void RuntimeInitialize(MethodBase method)
        {
            /*_ctrlInst = MonitoringControllerWrapper.MonitoringController;
            _cfRegistry = ControlFlowRegistry.Instance;*/
        }

        /// <summary >
        /// Method executed <b> before </b> the body of methods to which
        /// this aspect is applied .
        /// </ summary >
        /// <param name =" args ">
        /// Event arguments specifying which method is being executed ,
        /// which are its arguments , etc .
        /// </param >
        public override void OnEntry(MethodExecutionArgs args)
        {
            StateInformation state = new StateInformation();

            ResponseTimeRecord record = new ResponseTimeRecord();
            
            record.setCallId(1L);
            record.setOperation(_fullMethodName);
            record.setProcessId("" + Process.GetCurrentProcess().Id);
            
            state.Record = record;
            state.StartTime = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;

            // Store record for use in OnExit().
            args.MethodExecutionTag = state;

           /* if (!_ctrlInst.isMonitoringEnabled())
            {
                return;
            }
            OperationExecutionRecord execData = InitExecutionData();
            int eoi = 0; // execution order index
            int ess = 0; // execution stack size
            if (execData.isEntryPoint())
            {
                _cfRegistry.StoreThreadLocalEoi(0);
                _cfRegistry.StoreThreadLocalEss(1);
            }
            else
            {
                eoi = _cfRegistry.IncrementAndRecallThreadLocalEoi();
                ess = _cfRegistry.RecallAndIncrementThreadLocalEss();
            }
            if ((eoi == -1) || (ess == -1))
            {
                Console.WriteLine("eoi and /or ess have invalid values :" + " eoi == " + eoi + " ess == " + ess);
                _ctrlInst.terminateMonitoring();
            }
            execData.setEoi(eoi);
            execData.setEss(ess);
            //execData .eoi = eoi;
            //execData .ess = ess;
            // Time when monitored method begins execution .
            //execData .tin = _ctrlInst . getTimeSource (). getTime ();
            execData.setTin(_ctrlInst.getTimeSource().getTime());
            // Store execData for use in OnExit ().
            args.MethodExecutionTag = execData;*/
        }


        /// <summary >
        /// Method executed <b>after </b> the body of methods to which this
        /// aspect is applied , even when the method exits with an
        /// exception ( this method is invoked from the <c> finally </c>
        /// block ).
        /// </ summary >
        /// <param name =" args ">
        /// Event arguments specifying which method is being executed and
        /// which are its arguments .
        /// </param >
        public override void OnExit(MethodExecutionArgs args)
        {
            // Restore record.
            StateInformation state = (StateInformation)args.MethodExecutionTag;
            ResponseTimeRecord record = state.Record;

            record.setTimeStamp((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds);
            record.setResponseTime(record.getTimeStamp() - state.StartTime);

            AIMnet.Instance.DataCollector.newRecord(record);

          /*  if (!_ctrlInst.isMonitoringEnabled())
            {
                return;
            }

            // Restore execData .
            OperationExecutionRecord execData = (OperationExecutionRecord)args.MethodExecutionTag;

            // Time the monitored method is finished .
            //execData . tout = _ctrlInst . getTimeSource (). getTime ();
            execData.setTout(_ctrlInst.getTimeSource().getTime());
            if (execData.isEntryPoint())
            {
                _cfRegistry.UnsetThreadLocalTraceId();
            }

            // Create a new monitoring record with the measured data .
            _ctrlInst.newMonitoringRecord(execData);
            if (execData.isEntryPoint())
            {
                _cfRegistry.UnsetThreadLocalEoi();
                _cfRegistry.UnsetThreadLocalEss();
            }
            else
            {
                _cfRegistry.StoreThreadLocalEss(execData.getEss());
            }*/
        }

        /// <summary >
        /// Initializes all relevant data for monitoring .
        /// </ summary >
        /// <returns >
        /// The <c> OperationExecutionRecord </c> for this monitoring
        /// session .
        /// </ returns >
        /*private OperationExecutionRecord InitExecutionData()
        {
            long traceId = _cfRegistry.RecallThreadLocalTraceId();
            OperationExecutionRecord execData = new OperationExecutionRecord(_componentName, _method, traceId);
            //execData . isEntryPoint = false ;
            execData.setEntryPoint(false);
            if (execData.getTraceId() == -1)
            {
                execData.setTraceId(_cfRegistry.GetAndStoreUniqueThreadLocalTraceId());
                execData.setEntryPoint(true);
                //execData . traceId = _cfRegistry . GetAndStoreUniqueThreadLocalTraceId ();
                //execData . isEntryPoint = true ;
            }
            execData.setHostName(_ctrlInst.getHostName());
            execData.setExperimentId(_ctrlInst.getExperimentId());
            //execData . hostName = _ctrlInst . getHostName ();
            //execData . experimentId = _ctrlInst . getExperimentId ();
            return execData;
        }*/

        /// <summary >
        /// Can be used to cut the names of generic types that contain the
        /// string " ‘1[...]".
        /// </ summary >
        /// <param name =" name ">
        /// The type name as string to be formatted .
        /// </param >
        /// <returns >The formatted type name as string .</ returns >
        private static string FormatType(string name)
        {
            if (name != null && name.Contains("‘"))
            {
                string[] names = name.Split(new char[] { '‘' });
                name = names[0];
            }
            return name;
        }


        /// <summary >
        /// For some reason , <code > args . Method .Name </ code > is ". ctor " for
        /// constructors , whereas there is no "." for all other methods .
        /// </ summary >
        /// <param name =" name "> The string to be formatted .</ param >
        /// <returns >The formatted string withoud a leading ".". </ returns >
        private static string FormatMethodName(string name)
        {
            if (name.IndexOf('.') == 0)
            {
                name = name.Substring(1);
            }
            return name;
        }

        /// <summary >
        /// Creates a string that contains the parameter types .
        /// </ summary >
        /// <param name =" parameters ">
        /// A method ’s list of parameters .
        /// </param >
        /// <returns >
        /// The formatted parameter list as string , e.g.
        /// <c >( System .Int32 , System . String ) </c >.
        /// </ returns >
        private static string FormatParameters(ParameterInfo[] parameters)
        {
            StringBuilder formattedParameters = new StringBuilder("(");
            for (int i = 0; i < parameters.Length; i++)
            {
                formattedParameters.Append(FormatType(parameters[i].ParameterType.FullName));
                if (parameters.Length > 1 && i < parameters.Length - 1)
                {
                    formattedParameters.Append(", ");
                }
            }
            formattedParameters.Append(")");
            return formattedParameters.ToString();
        }
    }
}
