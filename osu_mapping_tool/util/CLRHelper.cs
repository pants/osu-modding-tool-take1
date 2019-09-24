using System;
using dnlib.DotNet;

namespace osu_mapping_tool.util
{
    public class ClrHelper
    {
        public static bool MatchesSig(MethodDef methodDef, params string[] sig)
        {
            var @params = methodDef.MethodSig.Params;
            if (@params.Count != sig.Length) return false;
            
            for (var i = 0; i < sig.Length; i++)
            {
                if (!@params[i].TypeName.Equals(sig[i]) && !sig[i].Equals("?"))
                    return false;
            }

            return true;
        }
        
        public static bool PrintSig(MethodDef methodDef)
        {
            var @params = methodDef.MethodSig.Params;
            
            Console.WriteLine(methodDef.Name + "(");
            foreach (var t in @params)
            {
                Console.Write(t + ",");
            }
            Console.WriteLine(")");

            return true;
        }
        
        public static bool MatchesSig(MethodSig methodSig, params string[] sig)
        {
            var @params = methodSig.Params;
            if (@params.Count != sig.Length) return false;
            
            for (var i = 0; i < sig.Length; i++)
            {
                if (!@params[i].TypeName.Equals(sig[i]) && !sig[i].Equals("?"))
                    return false;
            }

            return true;
        }
    }
}