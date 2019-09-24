using System;
using System.Collections.Generic;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassSongSelection : ClassMap
    {
        public ClassSongSelection() : base("GuiSongSelection")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            foreach (var method in methodDefs)
            {
                var strings = GetMethodStrings(method);

                if (!strings.Contains("stars") || !strings.Contains("length")
                    || !strings.Contains("keys") || !strings.Contains("played")) continue;

                var @class = MappingManager.GetClass(UnobfuscatedName)
                    .OrElse(new ClassObj(UnobfuscatedName, className));
                MappingManager.AddClass(@class);
            }
        }

        public override bool IsClass()
        {
            return true;
        }
    }
}