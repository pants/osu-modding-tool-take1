using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassUrls : ClassMap
    {
        public ClassUrls() : base("Urls")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            foreach (var method in methodDefs)
            {
                var strings = GetMethodStrings(method);

                if (!strings.Any(s => s.Contains("osu-search.php"))) continue;

                strings.ForEach(s => Console.WriteLine("> " + s));
                Console.WriteLine(">>>>>>>> " + className + " :: " + method.Name);
//                var @class = MappingManager.GetClass(UnobfuscatedName)
//                    .OrElse(new ClassObj(UnobfuscatedName, className));
//                MappingManager.AddClass(@class);
            }
        }
    }
}