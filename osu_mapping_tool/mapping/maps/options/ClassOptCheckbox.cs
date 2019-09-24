using System.Collections.Generic;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping_tool.util;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassOptCheckbox : ClassMap
    {
        public ClassOptCheckbox() : base("OptionCheckbox")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            foreach (var method in methodDefs)
            {
                if (method.Name.Equals(".ctor"))
                {
                    if (ClrHelper.MatchesSig(method, "String", "String", "Boolean", "EventHandler"))
                    {
                        MappingManager.AddClass(new ClassObj("OptionsCheckbox", className));
                    }
                }
            }
        }

        public override bool IsClass()
        {
            return true;
        }
    }
}