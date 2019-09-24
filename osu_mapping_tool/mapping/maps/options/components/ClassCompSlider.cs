using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping_tool.util;

namespace osu_mapping_tool.mapping.maps.options.components
{
    public class ClassCompSlider : ClassMap
    {
        public ClassCompSlider() : base("CompSlider")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            var sigCheck = methodDefs.Where(m =>
                ClrHelper.MatchesSig(m, "?", "Double", "Double", "Double", "Vector2", "Int32")).ToList();

            if (!sigCheck.Any()) return;

            var clazz = new ClassObj(UnobfuscatedName, className);
            MappingManager.AddClass(clazz);

            var callbackSearch = methodDefs
                .Where(m => ClrHelper.MatchesSig(m, "Object", "EventArgs")).ToList();
            if (!callbackSearch.Any()) return;

            foreach (var methodDef in callbackSearch)
            {
                var insn = GetOpcodePattern(methodDef, 0, OpCodes.Ldfld, OpCodes.Brfalse_S, OpCodes.Ldarg_0,
                    OpCodes.Ldfld, OpCodes.Ldc_I4_1, OpCodes.Callvirt);

                if (!insn.IsPresent()) continue;

                var i = insn.Get();
                var fieldDef = i.Operand as FieldDef;
                clazz.AddField("SliderCallbacks", fieldDef.Name);

                var sliderChangeClass = new ClassObj("SliderChanged", fieldDef.FieldType.FullName);
                MappingManager.AddClass(sliderChangeClass);
            }

            FindSetTooltip(clazz, methodDefs);
            FindValueField(clazz, methodDefs);
        }

        private void FindValueField(ClassObj clazz, IList<MethodDef> methodDefs)
        {
            var methodContaingValueFld = methodDefs.First(m => ClrHelper.MatchesSig(m, "Double", "Boolean", "Boolean"));
            var pattern = GetOpcodePattern(methodContaingValueFld, 1, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldarg_1, OpCodes.Beq_S);
            if (!pattern.IsPresent())
            {
                Console.WriteLine("Pattern not found while finding valueFld");
                return;
            }
            
            clazz.AddField("SliderValue", (pattern.Get().Operand as FieldDef).Name);
        }

        private void FindSetTooltip(ClassObj classObj, IList<MethodDef> methodDefs)
        {
            var setTooltipMethod = methodDefs.First(m => ClrHelper.MatchesSig(m, "String"));
            classObj.AddMethod("SetTooltip", setTooltipMethod.Name);
        }
    }
}