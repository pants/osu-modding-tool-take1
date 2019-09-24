using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Annex_Patcher.injection.classes
{
    public class OptionsSyringe : Syringe
    {
        public OptionsSyringe() : base("GuiOptions")
        {
            AddMethod("OnInit", InjectInit);
        }

        private void InjectInit(MethodDef method)
        {
            var mod = Patcher.osu_exe;
            var insn = method.Body.Instructions;
            
            var eventCtor = new MemberRefUser(mod, ".ctor",
                MethodSig.CreateInstance(mod.CorLibTypes.Void), //Arg 0, Boolean state
                mod.Find("Annex.eventmanager.events.OptionsInitializeEvent", false));


            
            //EventManager.Invoke(this, new OptionsInitializeEvent)
            insn.Insert(insn.Count - 1, OpCodes.Ldarg_0.ToInstruction()); //this,
            insn.Insert(insn.Count - 1, OpCodes.Newobj.ToInstruction(eventCtor)); //new OptionsInitializeEvent
            
            var invokeMethod = mod.Find("Annex.eventmanager.EventManager", false).FindMethod("Invoke");
            insn.Insert(insn.Count - 1,  OpCodes.Call.ToInstruction(invokeMethod)); //call method with above args
        }
    }
}