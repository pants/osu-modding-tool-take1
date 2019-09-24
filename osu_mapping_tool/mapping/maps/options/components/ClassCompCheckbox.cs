using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping_tool.util;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassCompCheckbox : ClassMap
    {
        public ClassCompCheckbox() : base("CompCheckbox")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            foreach (var method in methodDefs)
            {
                if (!method.Name.Equals(".ctor")) continue;
                if (!HasStrings(method, "circle-full")
                    && !ClrHelper.MatchesSig(method, "String", "Single", "Vector2", "Single", "Boolean", "Single"))
                    continue;
                ClrHelper.PrintSig(method);
                var clazz = new ClassObj("CompCheckbox", className);
                MappingManager.AddClass(clazz);
                FindComponentList(clazz, type.Fields);
                FindAddCallback(clazz, methodDefs);
            }
        }

        private void FindComponentList(ClassObj clazz, IList<FieldDef> typeFields)
        {
            foreach (var typeField in typeFields)
            {
                if (!typeField.FullName.Contains("Generic.List")) continue;
                clazz.AddField("ComponentList", typeField.Name);
                return;
            }
        }

        private static void FindAddCallback(ClassObj clazz, IList<MethodDef> methodDefs)
        {
            foreach (var method in methodDefs)
            {
                var insn = method.Body.Instructions;
                var isAddCallback = insn.Any(f => f.ToString().Contains("Delegate::Combine"));
                if (!isAddCallback) continue;
                
                clazz.AddMethod("AddToggleCallback", method.Name);
                
                MappingManager.AddClass(new ClassObj("CheckboxCheckedDelegate", method.MethodSig.Params[0].FullName));
                return;
            }
        }

        public override bool IsClass()
        {
            return true;
        }
    }
}