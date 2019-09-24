using System;
using System.Collections.Generic;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;

namespace Annex_Patcher.injection
{
    public abstract class Syringe
    {
        public ClassObj ClassObj;
        public string ClassName = string.Empty;

        private readonly List<Tuple<string, MethodSyringe>> _methodInjectors;

        protected delegate void InjectMethod(MethodDef def);

        protected Syringe(string clazz, bool classObj = true)
        {
            _methodInjectors = new List<Tuple<string, MethodSyringe>>();
            if (classObj)
            {
                var classObjOpt = MappingManager.GetClass(clazz);
                if (!classObjOpt.IsPresent())
                {
                    Console.WriteLine("Syringe ClassObj not found: " + clazz);   
                }
                ClassObj = classObjOpt.Get();
            }
            else
            {
                ClassName = clazz;
            }
        }

        public void InjectClass(TypeDef type)
        {
            if (ClassObj != null && !type.Name.Equals(ClassObj.ObfName)) return;
            if (ClassObj == null && !type.Name.Equals(ClassName)) return;

            Inject(type);
            Console.WriteLine("Class Injected: " + GetClassName());
            InjectMethods(type);
        }

        private void InjectMethods(TypeDef type)
        {
            foreach (var methodDef in type.Methods)
            {
                _methodInjectors.ForEach(m =>
                {
                    var methodObjOpt = ClassObj.GetMethod(m.Item1);
                    if (!methodObjOpt.IsPresent() || !methodObjOpt.Get().ObfName.Equals(methodDef.Name)) return;

                    m.Item2.Invoke(methodDef);
                    Console.WriteLine("... Method Injected: " + methodObjOpt.Get().Name);
                });
            }
        }

        protected void AddMethod(string name, MethodSyringe method)
        {
            _methodInjectors.Add(Tuple.Create<string, MethodSyringe>(name, method));
        }

        protected virtual void Inject(TypeDef t)
        {
        }

        public string GetClassName()
        {
            return ClassObj != null ? ClassObj.Name : ClassName;
        }

        public delegate void MethodSyringe(MethodDef methodDef);
    }
}