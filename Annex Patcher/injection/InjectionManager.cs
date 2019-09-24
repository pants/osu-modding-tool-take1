using System.Collections.Generic;
using Annex_Patcher.injection.classes;

namespace Annex_Patcher.injection
{
    public class InjectionManager
    {
        public void InjectCode()
        {
            var injectors = new List<Syringe>()
            {
                new OsuSyringe(),
                new OptionsSyringe(),
                new GameMainSyringe(),
                new HitObjectManagerSyringe(),
                new WebRequestSyringe(),
                new StringDeobfSyringe(),
                new InGameSyringe()
            };
            
            foreach (var type in Patcher.osu_exe.GetTypes())
            {   
                injectors.ForEach(i => i.InjectClass(type));
            }
        }
    }
}