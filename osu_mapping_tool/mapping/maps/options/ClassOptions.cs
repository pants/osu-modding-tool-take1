using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping_tool.util;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassOptions : ClassMap
    {
        public ClassOptions() : base("GuiOptions")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            var stringList = new List<string>();

            foreach (var method in methodDefs)
            {
                MethodDef addMethod = null;
                if (method.Body != null && method.Body.Instructions != null)
                {
                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        var previnsn = i == 0 ? null : method.Body.Instructions[i - 1];
                        var insn = method.Body.Instructions[i];
                        var nextinsn = i + 1 == method.Body.Instructions.Count ? null : method.Body.Instructions[i + 1];

                        if (insn.OpCode == OpCodes.Call && previnsn != null && previnsn.OpCode == OpCodes.Ldloc_1)
                        {
                            if (insn.Operand is IMethodDefOrRef)
                            {
                                var callingMethod = insn.Operand as IMethodDefOrRef;
                                if (ClrHelper.MatchesSig(callingMethod.MethodSig, "?"))
                                {
                                    addMethod = callingMethod.ResolveMethodDef();
                                }
                            }
                        }

                        if (insn.OpCode != OpCodes.Call || previnsn == null || previnsn.OpCode != OpCodes.Ldc_I4)
                            continue;

                        if (!insn.ToString().EndsWith(Program.DeobfuscatorInsn)) continue;


                        var str = StringDeobfuscator.Deobfuscate((int) previnsn.Operand);
                        stringList.Add(str);
                    }
                }

                if (IsInitOpts(stringList))
                {
                    var @class = MappingManager.GetClass("GuiOptions").OrElse(new ClassObj("GuiOptions", className));
                    MappingManager.AddClass(@class);
                    @class.AddMethod("OnInit", method.Name);

                    if (addMethod != null)
                    {
                        @class.AddMethod("AddOption", addMethod.Name);
                    }
                }

                stringList.Clear();
            }
        }

        public override bool IsClass()
        {
            return Methods.Count > 1;
        }

        private static bool IsInitOpts(ICollection<string> str)
        {
            return str.Contains("English") && str.Contains("Nederlands") && str.Contains("letterboxing");
        }
    }
}