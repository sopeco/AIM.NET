using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIM_NET.Aspects
{
    [PSerializable]
    public class TestAspect : AbstractMethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            Console.WriteLine("INVOKE: " + FullMethodName);

            base.OnInvoke(args);
        }
    }
}
