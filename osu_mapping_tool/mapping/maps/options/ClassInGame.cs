using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using osu_mapping.objects;
using osu_mapping_tool.util;
using OpCodes = dnlib.DotNet.Emit.OpCodes;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassInGame : ClassMap
    {
        
        
        public ClassInGame() : base("InGame")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            ClassObj @class;
            if ((@class = FindByStrings(className, methodDefs, "rankletter", "ranking-", "-small")) == null) return;

            foreach (var methodDef in methodDefs)
            {
                var instructions = methodDef.Body.Instructions;
                FindScoreSubmission(@class, methodDef, instructions);
                FindCurrentScoreField(@class, instructions);
            }
        }

        private static void FindScoreSubmission(ClassObj @class, MethodDef methodDef, IList<Instruction> instructions)
        {
            var containsExit = false;
            var containsDateTime = false;

            foreach (var insn in instructions)
            {
                if(insn.OpCode != OpCodes.Call) continue;
                if (insn.ToString().EndsWith("System.Environment::Exit(System.Int32)"))
                {
                    containsExit = true;
                }
                if (containsExit && insn.ToString().Contains("System.DateTime::get_Now()"))
                {
                    containsDateTime = true;
                }
            }

            if (containsDateTime)
            {
                @class.AddMethod("SubmitScore", methodDef.Name);
            }
        }
        
        private static void FindCurrentScoreField(ClassObj @class, IList<Instruction> instructions)
        {
            if(@class.GetField("CurrentScore").IsPresent()) return;
            if (instructions.Count < 15) return;
            
            for (var i = 2; i < instructions.Count - 1; i++)
            {
                var prevPrevInsn = instructions[i - 2];
                var prevInsn = instructions[i - 1];
                var insn = instructions[i];
                var nextInsn = instructions[i + 1];

                if (prevPrevInsn.OpCode != OpCodes.Call || prevInsn.OpCode != OpCodes.Ldsfld ||
                    nextInsn.OpCode != OpCodes.Ret || !(prevInsn.Operand is IField)) continue;
                    
                var ldsfld = (IField) prevInsn.Operand;
                @class.AddField("CurrentScore", ldsfld.Name);
            }
        }
    }
}