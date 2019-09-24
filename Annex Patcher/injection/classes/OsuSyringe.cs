using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.Threading;

namespace Annex_Patcher.injection.classes
{
    public class OsuSyringe : Syringe
    {
        public OsuSyringe() : base("Osu")
        {
            AddMethod("Main", InjectMain);
            AddMethod("ExePath", InjectExePath);
        }

        private void InjectExePath(MethodDef method)
        {
            for (var i = 0; i < method.Body.Instructions.Count; i++)
            {
                var insn = method.Body.Instructions[i];
                if (!insn.ToString().EndsWith("System.Windows.Forms.Application::get_ExecutablePath()")) continue;
                method.Body.Instructions.Set(i, OpCodes.Ldstr.ToInstruction($@"{Patcher.osu_path}osu!.exe"));
            }
        }

        private void InjectMain(MethodDef method)
        {
            //This will put a method called at the start of osu!
            var initAnnex = Patcher.osu_exe.Find("Annex.Annex", false).FindMethod("Init");
            method.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(initAnnex));

            var newInsn = method.Body.Instructions[2].Clone();
            newInsn.OpCode = OpCodes.Ldsfld;
            var id = method.Body.Instructions.Count - 2;
            method.Body.Instructions.Insert(id++, OpCodes.Ldc_I4_0.ToInstruction());
            method.Body.Instructions.Insert(id++, OpCodes.Ldstr.ToInstruction($"{Patcher.osu_path}osu!.exe"));
            method.Body.Instructions.Insert(id++, OpCodes.Stelem_Ref.ToInstruction());
            method.Body.Instructions.Insert(id, newInsn);

            for (var i = 0; i < method.Body.Instructions.Count; i++)
            {
                var insn = method.Body.Instructions[i];
                if (!insn.ToString().Contains("SetCompatibleTextRenderingDefault")) continue;
                method.Body.Instructions.RemoveAt(i);
                method.Body.Instructions.RemoveAt(i - 1);
                break;
            }

            var remove = false;

            int currRet = 0, lastRet = 0;

            for (var i = 0; i < method.Body.Instructions.Count; i++)
            {
                var insn = method.Body.Instructions[i];

                if (remove)
                {
                    method.Body.Instructions.RemoveAt(i--);
                }

                if (insn.OpCode == OpCodes.Call)
                {
                    if (insn.ToString().EndsWith("set_CurrentDirectory(System.String)"))
                    {
                        remove = true;
                    }

                    if (insn.ToString().EndsWith("Environment::Exit(System.Int32)"))
                    {
                        remove = false;
                    }
                }

                if (insn.OpCode != OpCodes.Ret) continue;
                lastRet = currRet;
                currRet = i;
            }

            //Ensures osu!plus.exe will only run if the file is named, osu_buddy.exe
            method.Body.Instructions.RemoveAt(lastRet + 2);
            method.Body.Instructions.RemoveAt(lastRet + 2);
            method.Body.Instructions.Insert(lastRet + 2, OpCodes.Ldstr.ToInstruction("osu!.exe")); //osu_buddy.exe
        }
    }
}