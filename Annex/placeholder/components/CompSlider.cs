using Microsoft.Xna.Framework;

namespace Annex.placeholder.components
{
    public class CompSlider
    {
        public CompSlider(ComponentManager manager, double minVal, double maxVal, double defVal, Vector2 pos, int size)
        {   
        }

        public void SetTooltip(string s){}
        
        public double SliderValue;
        
        public SliderChanged SliderCallbacks;
    }
}