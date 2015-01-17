using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AIM_NET.Aspects
{
    /// <summary>
    /// Super class for MethodAspects
    /// </summary>
    [Serializable]
    public abstract class AbstractMethodAspect : OnMethodBoundaryAspect
    {
        /// <summary>
        /// The full name of the class that the target class has to be implemented.
        /// </summary>
        public string RequiredSuper
        {
            get;
            set;
        }

        /// <summary>
        /// The assembly which contains the specified super class.
        /// </summary>
        public string RequiredSuperAssembly
        {
            get;
            set;
        }

        /// <summary>
        /// Flag if the target methods have to be synchronized.
        /// </summary>
        public bool RequiredSynchronized
        {
            get;
            set;
        }

        /// <summary>
        /// Builds an abbreviated string consists of the given method's namespace and name.
        /// </summary>
        /// <param name="method">Source method</param>
        /// <returns></returns>
        private string MethodToString(MethodBase method)
        {
            string methodName = method.DeclaringType.FullName + ":" + method.Name + "(";
            bool first = true;
            foreach (ParameterInfo pi in method.GetParameters())
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    methodName += ", ";
                }
                methodName += pi.ParameterType.Name;
            }
            methodName += ")";
            return methodName;
        }

        public override bool CompileTimeValidate(MethodBase method)
        {
            // Flag whether the aspect should be applied
            bool applyAspect = true;

            // Check if a spezial super class is required
            if (RequiredSuper != null)
            {
                string[] superTypes = RequiredSuper.Split(';');
                string[] superAssemblies = RequiredSuperAssembly.Split(';');

                if (superTypes.Length != superAssemblies.Length)
                {
                    throw new ArgumentException("It have to be specified the same amount of assemblies as types.");
                }

                // Set default rule "FALSE", if at least one type was specified
                if (superTypes.Length > 0)
                {
                    applyAspect = false;
                }

                for (int i = 0; i < superTypes.Length; i++)
                {
                    // Skip if no type was specified
                    if (superTypes[i].Length <= 0)
                    {
                        continue;
                    }

                    Type supType;
                    // Load type of class/interface (and load assembly if required)
                    if (superAssemblies[i].Length > 0)
                    {
                        Assembly supAssembly = Assembly.Load(superAssemblies[i]);
                        supType = supAssembly.GetType(superTypes[i]);
                    }
                    else
                    {
                        supType = Type.GetType(superTypes[i]);
                    }

                    // Skip if no class/interface was found
                    if (supType == null)
                    {
                        Message.Write(method, SeverityType.ImportantInfo, "INFO", "No class/interface '" + superTypes[i] + "' found.");
                        continue;
                    }

                    // Store the result
                    if (supType.IsAssignableFrom(method.DeclaringType))
                    {
                        applyAspect = true;
                        break;
                    }
                    else
                    {
                        applyAspect = false;
                    }
                }
            }

            // Check for synchronized if needed
            if (RequiredSynchronized)
            {
                MethodImplAttributes mia = method.GetMethodImplementationFlags();
                bool isSynchronized = (mia & MethodImplAttributes.Synchronized) > 0;

                applyAspect = applyAspect && isSynchronized;
            }
            
            // Print an info wheter the aspect will be applied to the method
            if (applyAspect)
            {
                Message.Write(method, SeverityType.ImportantInfo, "INFO", "Applying '" + chopNamespace(GetType().Namespace) + "." + GetType().Name + "' to '" + MethodToString(method) + "'");
            }

            return applyAspect;
        }

        /// <summary>
        /// Chop the given namespace.
        /// </summary>
        /// <param name="ns">Namespace which will be shortened</param>
        /// <returns></returns>
        private string chopNamespace(string ns)
        {
            string[] splitted = ns.Split('.');
            string ret = "";
            foreach (string s in splitted)
            {
                if (ret.Length > 0)
                {
                    ret += ".";
                }
                ret += s.Substring(0, 1);
            }
            return ret;
        }
    }
}
