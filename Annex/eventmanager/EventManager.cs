using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Annex.eventmanager.imp;

namespace Annex.eventmanager
{
    public class EventManager
    {
        private static readonly List<RuntimeMethod> _listeners = new List<RuntimeMethod>();

        /// <summary>
        /// Registers a listener class for events
        /// </summary>
        /// <param name="listener">Instance of the class to register as a listener</param>
        public static void RegisterListener(IListener listener)
        {
            foreach (var method in listener.GetType().GetMethods())
            {
                if (method.GetParameters().Length != 1) continue;

                foreach (var attr in method.GetCustomAttributes(false))
                {
                    if (attr.GetType() != typeof(EventMethod)) continue;
                    _listeners.Add(new RuntimeMethod(listener, method));
                }
            }
        }

        public static void Invoke(object calling, IEvent e)
        {
            _listeners.ForEach(ev => ev.Invoke(e));

            if (calling != null)
                e.PostFire(calling);
        }

        public static void Invoke(IEvent e)
        {
            Invoke(null, e);
        }

        private class RuntimeMethod
        {
            private MethodInfo Method { get; }
            private object Instance { get; }

            public RuntimeMethod(object instance, MethodInfo method)
            {
                Instance = instance;
                Method = method;
            }

            public void Invoke(IEvent e)
            {
                if (Method.GetParameters()[0].ParameterType == e.GetType())
                    Method.Invoke(Instance, new object[] {e});
            }
        }
    }
}