using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping_tool.util;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassCompRect : ClassMap
    {
        public ClassCompRect() : base("CompRect")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            var rect = methodDefs.Where(m => ClrHelper.MatchesSig(m, "Vector2", "Vector2", "Single", "Color"));
            if (!rect.Any()) return;
            
            var clazz = new ClassObj("CompRect", className);
            MappingManager.AddClass(clazz);
        }
    }
}