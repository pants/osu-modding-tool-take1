using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping.util;
using osu_mapping_tool.util;

namespace osu_mapping_tool.mapping
{
    public abstract class ClassMap
    {
        public List<MethodObj> Methods = new List<MethodObj>();
        public List<FieldObj> Fields = new List<FieldObj>();
        public string UnobfuscatedName { get; } 

        protected ClassMap(string name)
        {
            this.UnobfuscatedName = name;
        }

        protected bool HasStrings(MethodDef methodDef, params string[] strings)
        {
            var methodStrings = GetMethodStrings(methodDef);
            return strings.All(s => methodStrings.Contains(s));
        }
        
        protected bool HasStrings(FieldDef fieldDef, params string[] strings)
        {
            var fieldStrings = GetFieldStrings(fieldDef);
            return strings.All(s => fieldStrings.Contains(s));
        }

        protected List<string> GetFieldStrings(FieldDef field)
        {
//            var stringList = new List<string>();
//
//            var value = field.GetType().GetField(field.Name).getV);

            return null;//stringList;
        }

        protected List<string> GetMethodStrings(MethodDef method)
        {
            var stringList = new List<string>();

            if (method.Body?.Instructions == null) return stringList;
            
            for (var i = 0; i < method.Body.Instructions.Count; i++)
            {
                var previnsn = i == 0 ? null : method.Body.Instructions[i - 1];
                var insn = method.Body.Instructions[i];

                if (insn.OpCode != OpCodes.Call || previnsn == null || previnsn.OpCode != OpCodes.Ldc_I4)
                    continue;

                if (!insn.ToString().EndsWith(Program.DeobfuscatorInsn)) continue;

                var str = StringDeobfuscator.Deobfuscate((int) previnsn.Operand);
                stringList.Add(str);
            }

            return stringList;
        }

        protected bool HasOpcodePattern(MethodDef methodDef, params OpCode[] opcodes)
        {
            return GetOpcodePattern(methodDef, 0, opcodes).IsPresent();
        }
        
        protected Optional<Instruction> GetOpcodePattern(MethodDef methodDef, int opcodeInsnId, params OpCode[] opcodes)
        {
            var insn = methodDef?.Body?.Instructions;
            if (insn == null) return Optional<Instruction>.Empty();
            
            for (var insnIndex = 0; insnIndex < insn.Count - opcodes.Length; insnIndex++)
            {
                var foundPattern = !opcodes.Where((t, opcodeIndex) => insn[insnIndex + opcodeIndex].OpCode != t).Any();
                if (foundPattern)
                    return Optional<Instruction>.Of(insn[insnIndex + opcodeInsnId]);
            }

            return Optional<Instruction>.Empty();
        }

        protected ClassObj FindByStrings(string className, IList<MethodDef> methodDefs, params string[] strings)
        {
            ClassObj @class = null;
            
            foreach (var method in methodDefs)
            {
                if(!HasStrings(method, strings)) continue;
               
                @class = MappingManager.GetClass(UnobfuscatedName)
                    .OrElse(new ClassObj(UnobfuscatedName, className));

                MappingManager.AddClass(@class);

                return @class;
                //@class.AddMethod("GetJoinedMods", method.Name); //Mods mods, bool abreviated, bool displayNone, bool parens, bool ?space?
            }

            return null;
        }

        protected ClassObj FindByStrings(string className, IList<FieldDef> fieldDefs, params string[] strings)
        {
            ClassObj @class = null;
            
            foreach (var method in fieldDefs)
            {
                if(!HasStrings(method, strings)) continue;
               
                @class = MappingManager.GetClass(UnobfuscatedName)
                    .OrElse(new ClassObj(UnobfuscatedName, className));

                MappingManager.AddClass(@class);

                return @class;
                //@class.AddMethod("GetJoinedMods", method.Name); //Mods mods, bool abreviated, bool displayNone, bool parens, bool ?space?
            }

            return null;
        }

        protected void AddMethod(string obfClassName, string methodName, string obfMethodName)
        {
            
        }

        public abstract void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs);

        public virtual void GetFields(string className, TypeDef type, IList<FieldDef> fieldDefs)
        {
        }

        public virtual bool IsClass()
        {
            return false;
        }
    }
}