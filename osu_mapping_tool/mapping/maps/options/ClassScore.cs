using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using osu_mapping;
using osu_mapping.objects;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassScore : ClassMap
    {
        public ClassScore() : base("Score")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            ClassObj @class = null;

            foreach (var method in methodDefs)
            {
                if (!HasStrings(method,
                    "chickenmcnuggets{0}o15{1}{2}smustard{3}{4}uu{5}{6}{7}{8}{9}{10}{11}Q{12}{13}{15}{14:yyMMddHHmmss}{16}")
                ) continue;

                @class = MappingManager.GetClass(UnobfuscatedName)
                    .OrElse(new ClassObj(UnobfuscatedName, className));

                MappingManager.AddClass(@class);
                //@class.AddMethod("GetJoinedMods", method.Name); //Mods mods, bool abreviated, bool displayNone, bool parens, bool ?space?
            }

            if (@class != null)
            {
                FindSubmitRequest(@class, methodDefs);
                FindFields(@class, methodDefs);
            }
        }

        private void FindSubmitRequest(ClassObj @class, IList<MethodDef> methodDefs)
        {
            foreach (var method in methodDefs)
            {
                if (!HasStrings(method, "https://osu.ppy.sh/web/osu-submit-modular.php"))
                    continue;
                
                @class.AddMethod("SubmitRequest", method.Name);
                return;
            }
        }

        private void FindFields(ClassObj @class, IList<MethodDef> methodDefs)
        {
            foreach (var method in methodDefs)
            {
                if (!HasStrings(method,
                    "{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}:{15}:{16:yyMMddHHmmss}:{17}"))
                    continue;

                int addedFields = 0;
                foreach (var insn in method.Body.Instructions)
                {
                    if (insn.OpCode != OpCodes.Ldfld || !(insn.Operand is IField)) continue;

                    var field = (IField) insn.Operand;
                    var fieldType = field.FieldSig.Type;
                    if (fieldType == Program.osu_exe.CorLibTypes.UInt16 ||
                        fieldType == Program.osu_exe.CorLibTypes.Int32)
                    {
                        //TODO: Doesn't get TotalScore because it's a 'callvirt' and not a 'Ldfld'
                        switch (addedFields)
                        {
                            case 0:
                                @class.AddField("Count300", field.Name);
                                break;
                            case 1:
                                @class.AddField("Count100", field.Name);
                                break;
                            case 2:
                                @class.AddField("Count50", field.Name);
                                break;
                            case 3:
                                @class.AddField("CountGeki", field.Name);
                                break;
                            case 4:
                                @class.AddField("CountKatu", field.Name);
                                break;
                            case 5:
                                @class.AddField("CountMiss", field.Name);
                                break;
                            case 6:
                                @class.AddField("CountMaxCombo", field.Name);
                                break;
                        }
                        addedFields++;
                    }
                }
            }
        }
    }
}