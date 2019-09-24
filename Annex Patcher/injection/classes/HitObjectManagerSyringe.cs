using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Annex_Patcher.injection.classes
{
    public class HitObjectManagerSyringe : Syringe
    {
        public HitObjectManagerSyringe() : base("HitObjectManager")
        {
            AddMethod("UpdateMapValues", InjectUpdateMapValues);
        }

        private void InjectUpdateMapValues(MethodDef methodDef)
        {
            var mod = Patcher.osu_exe;
            var hookClass = mod.Find("Annex.hooks.MapSettingHook", false);
            var arMultiGet = hookClass.FindMethod("GetApproachRate");
            var csMultiGet = hookClass.FindMethod("GetCircleSize");
            var odMultiGet = hookClass.FindMethod("GetOverallDifficulty");

            var instructions = methodDef.Body.Instructions;

            var amt = 0;

            for (var i = 0; i < instructions.Count; i++)
            {
                var insn = instructions[i];

                if (amt >= 4) break;
                if (insn.GetOpCode() != OpCodes.Ldfld || (!insn.ToString().Contains("System.Single"))) continue;
                instructions.Insert(i + 1, OpCodes.Call.ToInstruction(odMultiGet));
                i += 2;
                amt++;
            }

            for (var i = 3; i < instructions.Count - 3; i++)
            {
                bool IsDouble(Instruction x) => x.OpCode == OpCodes.Ldc_R8;
                double Val(Instruction x) => (double) x.Operand;

                var insn = instructions[i];
                var insnP1 = instructions[i + 1];
                var insnP2 = instructions[i + 2];
                if (!IsDouble(insn) || !IsDouble(insnP1) || !IsDouble(insnP2)) continue;
                if (Val(insn) != 1800f || Val(insnP1) != 1200f || Val(insnP2) != 450f) continue;

                instructions.Insert(i - 1, OpCodes.Call.ToInstruction(arMultiGet));
                instructions.Insert(i + 19, OpCodes.Call.ToInstruction(csMultiGet)); //CircleSize
                return;
            }
        }
    }
}