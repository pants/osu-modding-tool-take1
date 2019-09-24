using System.Collections.Generic;
using dnlib.DotNet;

namespace osu_mapping_tool.mapping.maps.options
{
    public class ClassPopupHandler : ClassMap
    {
        public ClassPopupHandler() : base("PopupHandler")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            throw new System.NotImplementedException();
        }
    }
}