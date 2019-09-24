using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using osu_mapping;
using osu_mapping.objects;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassGameMain : ClassMap
    {
        public ClassGameMain() : base("GameHelper")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            foreach (var methodDef in methodDefs)
            {
                if (!HasStrings(methodDef, "Are you sure you want to exit osu!?")) continue;

                var @class = MappingManager.GetClass(UnobfuscatedName)
                    .OrElse(new ClassObj(UnobfuscatedName, className));
                MappingManager.AddClass(@class);

                @class.AddMethod("ExitGame", methodDef.Name); //methodSig (1): bool
                var insn = methodDef.Body.Instructions;
                if (!(insn[insn.Count - 2].Operand is MethodDef showScreenDef))
                {
                    Console.WriteLine("Can't get ShowScreen");
                    return;
                }

                @class.AddMethod("ShowScreen", showScreenDef.Name); //methodSig (1): GuiScreen

                FindKeyPressMethod(@class, methodDefs);
                return;
            }
        }

        private void FindKeyPressMethod(ClassObj classObj, IList<MethodDef> methodDefs)
        {
            var keyPressMethod = methodDefs.First(m => HasStrings(m, "Skin reload queued."));
            classObj.AddMethod("OnKeyPress", keyPressMethod.Name);
        }
    }
}