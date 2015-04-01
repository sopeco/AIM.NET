using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIM_NET.Aspects
{
    /// <summary>
    /// Writes a string to the console, when the target method is invoked.
    /// </summary>
    [PSerializable]
    public class TestAspect : AbstractMethodInterceptionAspect
    {
        /// <summary>
        /// This method is invoked when the instrumented target method is invoked.
        /// </summary>
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            Console.WriteLine("Invoking: " + FullMethodName);
            base.OnInvoke(args);
        }
    }
}
