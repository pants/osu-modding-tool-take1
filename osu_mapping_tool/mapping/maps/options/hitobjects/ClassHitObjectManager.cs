using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using osu_mapping;
using osu_mapping.objects;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassHitObjectManager : ClassMap
    {
        public ClassHitObjectManager() : base("HitObjectManager")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            ClassObj @class;
            if ((@class = FindByStrings(className, methodDefs, "hit0", "hit300", "spinner-osu", "particle")) == null) return;

            foreach (var methodDef in methodDefs)
            {
                if(!methodDef.HasBody) continue;
                var instructions = methodDef.Body.Instructions;
                if(instructions.Count < 7) continue;
                
                for (var i = 3; i < instructions.Count - 3; i++)
                {
                    bool IsDouble(Instruction x) => x.OpCode == OpCodes.Ldc_R8;
                    double Val(Instruction x) => (double)x.Operand;
                    var insn = instructions[i];
                    var insnP1 = instructions[i + 1];
                    var insnP2 = instructions[i + 2];
                    if (!IsDouble(insn) || !IsDouble(insnP1) || !IsDouble(insnP2)) continue;
                    if (Val(insn) != 1800f || Val(insnP1) != 1200f || Val(insnP2) != 450f) continue;                  
                    @class.AddMethod("UpdateMapValues", methodDef.Name);
                    var field = instructions[i + 5].Operand as FieldDef;
                    Console.WriteLine("PreEmpt" + field.FullName);
                    
                    var @parentClass = MappingManager.GetClass("HitObjectManagerParent")
                        .OrElse(new ClassObj("HitObjectManagerParent", field.DeclaringType.Name));
            
                    MappingManager.AddClass(@parentClass);
                    @parentClass.AddField("ApproachRatePrecise", field.Name);
                    
                    
                    return;
                }
            }
        }
    }
}