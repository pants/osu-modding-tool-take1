using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Annex.placeholder.components
{
    public class CompCheckbox
    {
        public List<object> ComponentList;
        
        public CompCheckbox(string text, float size, Vector2 position, float depth, bool defaultState,
            float maxWidth = 0)
        {
        }

        public void AddToggleCallback(CheckboxCheckedDelegate callback)
        {}
    }
}