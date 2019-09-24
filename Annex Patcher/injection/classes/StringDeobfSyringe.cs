using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Annex_Patcher.injection.classes
{
    public class StringDeobfSyringe : Syringe
    {
        public StringDeobfSyringe() : base("StringDeobf")
        {
            AddMethod("Deobfuscate", InjectDeobfuscate);
        }

        private void InjectDeobfuscate(MethodDef method)
        {
            var mod = Patcher.osu_exe;

            if (method.Body.Instructions == null)
            {
                return;
            }

            var insn = method.Body.Instructions;

            var invokeMethod = mod.Find("Annex.hooks.StringDeobfuscatorHook", false).FindMethod("ReplaceString");
            insn.Insert(insn.Count - 1, OpCodes.Call.ToInstruction(invokeMethod)); //call method with above args
        }
    }
}