using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;

namespace osu_mapping_tool.mapping.maps.options.hitobjects
{
    public class ClassHitObjectSlider : ClassMap
    {
        public ClassHitObjectSlider() : base("HitObjectSlider")
        {
        }

        public override void GetMethods(string className, TypeDef type, IList<MethodDef> methodDefs)
        {
            var methods = methodDefs.Where(m => HasStrings(m, "sliderscorepoint"));
            if (!methods.Any()) return;
        }
    }
}