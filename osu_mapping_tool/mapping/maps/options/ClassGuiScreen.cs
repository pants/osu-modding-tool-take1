using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using osu_mapping;
using osu_mapping.objects;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassGuiScreen : ClassMap
    {
        public ClassGuiScreen() : base("GuiScreen")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            var keys =
                from method in methodDefs
                let strings = GetMethodStrings(method)
                where strings.Count != 0
                where strings.Contains("D") || strings.Contains("")
                select method.Body.Instructions.Where(i => i.ToString().Contains("Microsoft.Xna.Framework.Input.Keys"));

            if (!keys.Any(insn => insn.Any())) return;

            var @class = MappingManager.GetClass(UnobfuscatedName)
                .OrElse(new ClassObj(UnobfuscatedName, className));

            MappingManager.AddClass(@class);
            FindComponentManager(methodDefs, @class);
        }

        private void FindComponentManager(IList<MethodDef> methodDefs, ClassObj @class)
        {
            var ctorList = methodDefs.Where(i => i.Name == ".ctor");
            if (!ctorList.Any()) return;
            var ctor = ctorList.First();

            var insnOpt = GetOpcodePattern(ctor, 4, OpCodes.Newobj, OpCodes.Dup, OpCodes.Ldc_I4_0, OpCodes.Stfld,
                OpCodes.Stfld);
            insnOpt.IfPresent(insn =>
            {
                if (!(insn.Operand is FieldDef field)) return;

                @class.AddField("componentManagerFld", field.Name);
                AddComponentManagerClass(field);
            });
        }

        private void AddComponentManagerClass(FieldDef field)
        {
            var componentManagerClass = MappingManager.GetClass("ComponentManager")
                .OrElse(new ClassObj("ComponentManager", field.FieldType.TypeName));
            MappingManager.AddClass(componentManagerClass);

            var compManager = Program.osu_exe.Find(field.FieldType.TypeName, false);
            foreach (var compManagerMethod in compManager.Methods)
            {
                var hasNullCheck = HasOpcodePattern(compManagerMethod, OpCodes.Ldarg_1, OpCodes.Brtrue_S, OpCodes.Ret);
                var hasCount = HasOpcodePattern(compManagerMethod, OpCodes.Callvirt, OpCodes.Ldc_I4_0, OpCodes.Ble_S);
                var hasGreaterZero = HasOpcodePattern(compManagerMethod, OpCodes.Ldloc_0, OpCodes.Ldc_I4_0, OpCodes.Bge_S);
                if(!hasNullCheck || !hasCount || !hasGreaterZero) continue;
                
                componentManagerClass.AddMethod("AddComponent", compManagerMethod.Name);
                AddDrawableClass(compManagerMethod);
                return;
            }
        }

        private void AddDrawableClass(MethodDef methodDef)
        {
            var drawableParam = methodDef.MethodSig.Params[0];
            Console.WriteLine("Drawable: " + methodDef.MethodSig.Params[0]);
            var drawableClass = MappingManager.GetClass("Drawable")
                .OrElse(new ClassObj("Drawable", drawableParam.TypeName));
            MappingManager.AddClass(drawableClass);
        }
    }
}