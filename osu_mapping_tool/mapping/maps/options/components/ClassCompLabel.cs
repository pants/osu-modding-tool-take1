using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping_tool.util;

namespace osu_mapping_tool.mapping.maps.options.components
{
    public class ClassCompLabel : ClassMap
    {
        public ClassCompLabel() : base("CompLabel")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            var HasCtor = methodDefs.Where(m => ClrHelper.MatchesSig(m, 
                "String", "Single", "Vector2", "Vector2", "Single", "Boolean", "Color", "Boolean"));
            var ContainsStrings = methodDefs.Where(m => HasStrings(m, "Text: "));

            //string text, float textSize, Vector2 startPosition, Vector2 bounds, float drawDepth, bool alwaysDraw, Color colour, bool shadow = true
            if (!HasCtor.Any() || !ContainsStrings.Any()) return;
            
            var clazz = new ClassObj("CompLabel", className);
            MappingManager.AddClass(clazz);

            var str = methodDefs.First(m => ClrHelper.MatchesSig(m, "String"));
            clazz.AddMethod("SetText", str.Name); //MethodSig (1): String
        }
    }
}