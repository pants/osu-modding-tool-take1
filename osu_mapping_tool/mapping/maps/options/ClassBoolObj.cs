using System;
using System.Collections.Generic;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassBoolObj : ClassMap
    {
        public ClassBoolObj() : base("BoolObj")
        {  
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            foreach (var methodDef in methodDefs)
            {
                if (!methodDef.Name.Equals("ToString") || !HasStrings(methodDef, "0", "1")) continue;
                
                var @class = MappingManager.GetClass("BoolObj").OrElse(new ClassObj("BoolObj", className));
                MappingManager.AddClass(@class);
            }
        }

    }
}