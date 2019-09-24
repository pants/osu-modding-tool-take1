using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping.util;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClasspWebRequest : ClassMap
    {
        private const string fallbackStr = "---------------------- USING FALLBACK PATH! ---------------------";
        private const string timeoutStr = "Request to {0} ({1}) failed with {2} (FAILED).";

        public ClasspWebRequest() : base("WebRequest")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            bool Contains(List<string> strings) => strings.Contains(fallbackStr) || strings.Contains(timeoutStr);
            
            if (MappingManager.Classes.Find(c => c.Name.Equals(UnobfuscatedName)) != null) return;
            if (!methodDefs.Select(GetMethodStrings).Any(Contains)) return;

            var @class = MappingManager.GetClass(UnobfuscatedName)
                .OrElse(new ClassObj(UnobfuscatedName, className));
            
            MappingManager.AddClass(@class);

            Optional<MethodDef>.OfNullable(methodDefs
                .First(method => method.Body.Instructions.Any(insn => insn.ToString().Contains("GetResponseStream"))))
                .IfPresent(m => @class.AddMethod("StartResponse", m.Name));
        }
    }
}