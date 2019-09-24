using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace osu_mapping_tool
{
    public class StringDeobfuscator
    {
        public static string DeobfClassName = "", //Class name for the string deobfuscation class
            DeobfMethodName = "", //Method name for the string deobfuscation method
            DeobfMainName = ""; //Method name for the method that calls the string deobfuscation method

        private static Type strClass = null;

        private static MethodInfo deobfMethod = null;
        //#=q1aOO8gYUxlcSjDXVSw3E40Lmr49F_4iJyE3ZrLRojzM=
        //#=zyyuEL5Q=

        public static string Deobfuscate(int num)
        {
            if (strClass == null)
            {
                var asm = Assembly.LoadFile(@"E:\osu!\osu!.exe");
                strClass = asm.GetType(DeobfClassName);
                RuntimeHelpers.RunClassConstructor(strClass.TypeHandle);

                Console.WriteLine("StrClass: " + (strClass == null));

                deobfMethod = strClass.GetMethod(DeobfMethodName, BindingFlags.Static | BindingFlags.NonPublic);

                Console.WriteLine(deobfMethod.ReturnType);
                Console.WriteLine("DeobfClass: " + (deobfMethod == null));
                // strClass.TypeInitializer.Invoke(null, null);
            }

            var o = new object[] {num, true};
            var ret = deobfMethod.Invoke(strClass, o);
            return ret as string;
            //return Class26.smethod_0(num);
        }

        public static void Init(ModuleDefMD osu_exe)
        {
            var entryPoint = osu_exe.EntryPoint;
            var firstInsn = entryPoint.Body.Instructions[0];
            var methodDef = firstInsn.Operand as MethodDef;
            
            var entryInsn = methodDef.Body.Instructions;

            Console.WriteLine("> Main osu! Method: " + methodDef.FullName);

            //Loop the osu! start method until we find LDC_I4, the string deobfuscation calling method is after it
            for (var i = 0; i < entryInsn.Count; i++)
            {
                var insn = entryInsn[i];
                if (insn.OpCode != OpCodes.Ldc_I4) continue;

                var strObf = entryInsn[i + 1].Operand as MethodDef;
                Console.WriteLine("> Main Deobfuscation Method: " + strObf.FullName);

                var strObfInsn = strObf.Body.Instructions;

                for (var i1 = 0; i1 < strObfInsn.Count; i1++)
                {
                    if (strObfInsn[i1].OpCode != OpCodes.Ldc_I4_1) continue;

                    var method = strObfInsn[i1 + 1].Operand as MethodDef;
                    Console.WriteLine("> Desired Deobfuscation Method: " + method.FullName);

                    DeobfClassName = method.DeclaringType.Name;
                    DeobfMainName = strObf.Name;
                    DeobfMethodName = method.Name;
                    return;
                }

                return;
            }
        }
    }
}