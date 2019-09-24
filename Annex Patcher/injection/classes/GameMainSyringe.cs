using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Annex_Patcher.injection.classes
{
    public class GameMainSyringe : Syringe
    {
        public GameMainSyringe() : base("GameHelper")
        {
            AddMethod("OnKeyPress", InjectOnKeyPress);
        }

        private void InjectOnKeyPress(MethodDef methoddef)
        {
            var mod = Patcher.osu_exe;
            var insn = methoddef.Body.Instructions;
           
            var eventCtor = mod.Find("Annex.eventmanager.events.KeyPressEvent", false).FindMethod(".ctor");
            
            //EventManager.Invoke(this, new OptionsInitializeEvent)
            insn.Insert(0, OpCodes.Ldarg_0.ToInstruction()); //this,
            insn.Insert(1, OpCodes.Ldarg_1.ToInstruction()); //arg
            insn.Insert(2, OpCodes.Newobj.ToInstruction(eventCtor)); //new KeyPressEvent(arg)
            
            var invokeMethod = mod.Find("Annex.eventmanager.EventManager", false).FindMethod("Invoke");
            insn.Insert(3,  OpCodes.Call.ToInstruction(invokeMethod)); //call method with above args
        }
    }
}