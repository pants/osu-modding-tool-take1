using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping_tool.util;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassModManager : ClassMap
    {
        public ClassModManager() : base("ModManager")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            ClassObj @class = null;
            
            foreach (var method in methodDefs)
            {
                var strings = GetMethodStrings(method);

                if(!ClrHelper.MatchesSig(method, "Mods", "Boolean", "Boolean", "Boolean", "Boolean"))
                    continue;
                
                if (!strings.Contains("DoubleTime") || !strings.Contains("DT")
                    || !strings.Contains("None") || !strings.Contains("Cinema")) continue;

                @class = MappingManager.GetClass(UnobfuscatedName)
                    .OrElse(new ClassObj(UnobfuscatedName, className));
                MappingManager.AddClass(@class);
                @class.AddMethod("GetJoinedMods", method.Name); //Mods mods, bool abreviated, bool displayNone, bool parens, bool ?space?
            }

            if(@class == null)
                return;
            
            foreach (var method in methodDefs)
            {
                if(!ClrHelper.MatchesSig(method, "Mods"))
                    continue;

                if (method.Body.Instructions.Count == 5)
                {
                    @class.AddMethod("IsModActive", method.Name); //Mods mods
                }
            }
        }

        public override bool IsClass()
        {
            throw new System.NotImplementedException();
        }
    }
}