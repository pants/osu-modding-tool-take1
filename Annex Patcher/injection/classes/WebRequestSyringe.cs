using dnlib.DotNet;

namespace Annex_Patcher.injection.classes
{
    public class WebRequestSyringe : Syringe
    {
        public WebRequestSyringe() : base("WebRequest")
        {
            AddMethod("StartResponse", InjectStartResponse);
        }

        private void InjectStartResponse(MethodDef method)
        {
            for (var i = 0; i < method.Body.Instructions.Count; i++)
            {
                var insn = method.Body.Instructions[i];
                if (!insn.ToString().Contains("GetResponseStream")) continue;
                method.Body.Instructions.RemoveAt(i + 2);
                method.Body.Instructions.RemoveAt(i + 2);
                method.Body.UpdateInstructionOffsets();
            }
        }
    }
}