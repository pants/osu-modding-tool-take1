using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Annex_Patcher.injection.classes
{
    public class InGameSyringe : Syringe
    {
        public InGameSyringe() : base("InGame")
        {
            AddMethod("SubmitScore", InjectSubmitScore);
        }

        private void InjectSubmitScore(MethodDef method)
        {
            var allowScoreSubmit = Patcher.osu_exe.Find("Annex.hooks.InGameHook", false)
                .FindMethod("ShouldAllowSubmission");

            var instructions = method.Body.Instructions;
            for (var i = 0; i < instructions.Count - 6; i++)
            {
                bool Valid(Instruction str) => str.OpCode == OpCodes.Brfalse;

                var insn = instructions[i];
                var insnP3 = instructions[i + 3];
                var insnP6 = instructions[i + 6];

                if (!Valid(insn) || !Valid(insnP3) || !Valid(insnP6)) continue;


                instructions.Insert(i + 7, OpCodes.Call.ToInstruction(allowScoreSubmit));
                instructions.Insert(i + 8, OpCodes.Brfalse.ToInstruction(insn.Operand as Instruction));
                return;
            }
        }
    }
}