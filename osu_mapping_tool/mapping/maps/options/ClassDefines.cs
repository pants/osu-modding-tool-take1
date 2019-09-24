using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping_tool.util;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassDefines : ClassMap
    {
        public ClassDefines() : base("Defines")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
          
//            if (className.Equals("#=zWA1KiDtrKaZ5t53FT9PHTg9zrxoH"))
//            {
//                foreach (var method in methodDefs)
//                {
//                    // if(!method.Name.Equals(".cctor")) return;
//                
//                    var strings = GetMethodStrings(method);
//                
//                    if(strings.Count == 0) continue;
//               
//
////                if (!strings.Contains("ppy.sh")) continue;
//
//                    Console.WriteLine(className);
//                    Console.WriteLine(method.FullName);
//                    Console.WriteLine("----");
//                    strings.ForEach(Console.WriteLine);
//                    Console.WriteLine("----");
//                }
//            }
            foreach (var method in methodDefs)
            {
                // if(!method.Name.Equals(".cctor")) return;

                var strings = GetMethodStrings(method);

                if (strings.Count == 0) continue;
                if (!strings.Contains("There was an error during timing calculations.  If you continue to get this error, please update your AUDIO/SOUND CARD drivers!  Your score will not be submitted for this play.")) continue;
                if (!strings.Contains(@"https://osu.ppy.sh/web/osu-submit-modular.p")) continue;
               // if (!strings.Contains("x2")) continue;
//                if (!strings.Contains("ppy.sh")) continue;
                // if(strings.Count(s => s.Contains("size") || s.Contains("bold") ) == 2) return;

                Console.WriteLine(className);
                Console.WriteLine(method.FullName);
                Console.WriteLine("----");
                strings.ForEach(Console.WriteLine);
                Console.WriteLine("----");
            }
        }

        public override void GetFields(string className, TypeDef type, IList<FieldDef> fieldDefs)
        {
//            if(fieldDefs.Count == 0) return;
//            
//            bool allStatic = false;
//
//            var testAcess = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly;
//
//            int not = fieldDefs.Count(fieldDef => (fieldDef.Attributes & FieldAttributes.Static) != 0 && fieldDef.FieldType == Program.osu_exe.CorLibTypes.String);
//
//            foreach (var fieldDef in fieldDefs)
//            {
//                fieldDef.
//            }
//            
//            if (not > 5)
//                Console.WriteLine(">>>>>>>>>> " + className);
        }
    }
}