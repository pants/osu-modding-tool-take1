using System;
using System.Reflection;

namespace Annex.util
{
    public static class ReflectionUtils
    {
        private const BindingFlags PRIVATE = BindingFlags.NonPublic | BindingFlags.Instance;

        public static void InvokeMethod(string className, string methodName, object classObj, object[] args)
        {
            var type = Type.GetType(className);
            var theMethod = type.GetMethod(methodName);
            theMethod.Invoke(classObj, args);
        }

        public static void InvokeMethod(MethodInfo method, object classObj, object[] args)
        {
            method.Invoke(classObj, args);
        }

        public static MethodInfo GetMethod(string className, string methodName)
        {
            var type = Type.GetType(className);
            return type.GetMethod(methodName);
        }

        public static MethodInfo GetMethodPrivate(string className, string methodName)
        {
            var type = Type.GetType(className);
            return type.GetMethod(methodName, PRIVATE);
        }

        public static MethodInfo GetMethodPrivate(Type type, string methodName)
        {
            return type.GetMethod(methodName, PRIVATE);
        }

        public static FieldInfo GetPrivateField(string className, string fieldName)
        {
            var fi = Type.GetType(className).GetField(fieldName, PRIVATE);
            return fi;
        }
    }
}