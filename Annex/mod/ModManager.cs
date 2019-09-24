using System;
using System.Collections.Generic;
using System.Linq;
using Annex.mod.mods;

namespace Annex.mod
{
    public class ModManager
    {
        private List<IMod> _mods = new List<IMod>()
        {
            new ModSettings(),
            new MapSettingsMod()
        };

        public void Initialize()
        {
            _mods.ForEach(m => m.OnInit());
        }

        public IMod GetMod(string modName)
        {
            return _mods.First(m => m.Name.ToLower().Equals(modName.ToLower()));
        }
        
        public IMod GetMod(Type modName)
        {
            return _mods.First(m => m.GetType() == modName);
        }
    }
}