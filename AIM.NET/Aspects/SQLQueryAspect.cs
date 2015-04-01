using AIM_NET;
using AIM_NET.Aspects;
using org.aim.artifacts.records;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AIM_NET.Aspects
{
    [PSerializable]
    public class SQLQueryAspect : AbstractMethodBoundaryAspect
    {

        public SQLQueryAspect()
        {
            AspectPriority = 100;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            //Console.WriteLine("> OnInvoke");
            SQLQueryRecord record = new SQLQueryRecord();

            record.setTimeStamp((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds);
            record.setOperation(FullMethodName);
            record.setProcessId("" + Process.GetCurrentProcess().Id);

            int queryIndex = -1;
            // TODO
            if (FullMethodName.Equals("System.Data.Entity.Database:ExecuteSqlCommand(String, Object[])"))
            {
                queryIndex = 0;
            }
            else if (FullMethodName.Equals("System.Data.Entity.Database:ExecuteSqlCommand(TransactionalBehavior, String, Object[])"))
            {
                queryIndex = 1;
            }
            else if (FullMethodName.Equals("System.Data.Entity.Database:SqlQuery(String, Object[])"))
            {
                queryIndex = 0;
            }
            
            if (queryIndex != -1)
            {
                record.setQueryString((string)args.Arguments[queryIndex]);
            }

            AIMnet.Instance.DataCollector.newRecord(record);
        }

    }
}
