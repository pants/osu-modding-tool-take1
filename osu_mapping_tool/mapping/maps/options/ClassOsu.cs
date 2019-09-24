using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using osu_mapping;
using osu_mapping.objects;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassOsu : ClassMap
    {
        public ClassOsu() : base("Osu")
        {
        }

        private void checkIfExePath(string className, MethodDef methodDef)
        {
            if (methodDef.Body?.Instructions == null) return;

            foreach (var insn in methodDef.Body.Instructions)
            {
                if (insn.OpCode != OpCodes.Call)
                    continue;
                
                if (!insn.ToString().EndsWith("System.Windows.Forms.Application::get_ExecutablePath()")) continue;

                var @class = MappingManager.GetClass(UnobfuscatedName)
                    .OrElse(new ClassObj(UnobfuscatedName, className));
                MappingManager.AddClass(@class);
               
                @class.AddMethod("ExePath", methodDef.Name);
            }
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            foreach (var methodDef in methodDefs)
            {
                checkIfExePath(className, methodDef);
                if (!HasStrings(methodDef, ".require_update", "help.txt", "Force update requested", "osu!.exe"))
                {
                    continue;
                }

                var @class = MappingManager.GetClass(UnobfuscatedName)
                    .OrElse(new ClassObj(UnobfuscatedName, className));
                MappingManager.AddClass(@class);
                @class.AddMethod("Main", methodDef.Name);
            }
        }

        public override bool IsClass()
        {
            throw new System.NotImplementedException();
        }
    }
}