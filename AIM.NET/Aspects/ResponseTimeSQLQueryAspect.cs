using org.aim.artifacts.records;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIM_NET.Aspects
{
    [PSerializable]
    public class ResponseTimeSQLQueryAspect : AbstractMethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            SQLQueryRecord sqlRecord = new SQLQueryRecord();
            sqlRecord.setTimeStamp((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds);
            sqlRecord.setOperation(FullMethodName);
            sqlRecord.setProcessId("" + Process.GetCurrentProcess().Id);

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
                sqlRecord.setQueryString((string)args.Arguments[queryIndex]);
            }
            else
            {
                sqlRecord.setQueryString(FullMethodName);
            }

            AIMnet.Instance.DataCollector.newRecord(sqlRecord);

            //////////////////////////////////////////////

            //Console.WriteLine("> OnInvoke");
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
