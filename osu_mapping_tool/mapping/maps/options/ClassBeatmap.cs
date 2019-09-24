using System;
using System.Collections.Generic;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping_tool.util;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassBeatmap : ClassMap
    {
        public ClassBeatmap() : base("Beatmap")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            ClassObj @class = null;
            
            foreach (var method in methodDefs)
            {
                if(!HasStrings(method, "Cannot convert a map to osz2 which does not have a Beatmapset ID.")) continue;
               
                @class = MappingManager.GetClass(UnobfuscatedName)
                    .OrElse(new ClassObj(UnobfuscatedName, className));

                MappingManager.AddClass(@class);
                //@class.AddMethod("GetJoinedMods", method.Name); //Mods mods, bool abreviated, bool displayNone, bool parens, bool ?space?
            }
        }
    }
}