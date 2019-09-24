using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping.util;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassAnticheat : ClassMap
    {
        public ClassAnticheat() : base("Anticheat")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            Console.WriteLine("wwww");
            bool Contains(List<string> strings) => strings.Contains("There was an error during timing calculations") 
                                                   || strings.Contains("user is hacking?");

            if (MappingManager.Classes.Find(c => c.Name.Equals(UnobfuscatedName)) != null) return;
            
            if (!methodDefs.Select(GetMethodStrings).Any(Contains)) return;

            var @class = MappingManager.GetClass(UnobfuscatedName)
                .OrElse(new ClassObj(UnobfuscatedName, className));
            
            MappingManager.AddClass(@class);

            Optional<MethodDef>.OfNullable(methodDefs.First(m => Contains(GetMethodStrings(m))))
                .IfPresent(m => @class.AddMethod("AudioCheck", m.Name));
        }
    }
}