using System;
using Annex.placeholder.components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Annex.gui.components
{
    public class ModStatComponent
    {
        public readonly CompLabel Label;
        public readonly CompSlider Slider;
        public readonly CompCheckbox Checkbox;

        private string _text;
        
        public event CheckboxUpdate CheckboxUpdateEvent;
        public event SliderUpdate SliderUpdateEvent;
        
        public ModStatComponent(ComponentManager manager, string s, int x, int y, bool defaultState, double min, double max, double def)
        {
            _text = s;
            Label = new CompLabel(s, 11, new Vector2(x, y), Vector2.Zero, 0.94F, true, Color.White, true); 
            manager.AddComponent(Label);
            Slider = new CompSlider(manager, min, max, def, new Vector2(x + 6, y + 20), 200);
            Checkbox = new CompCheckbox("Enable " + s + " Mod", 0.7f, new Vector2(x + 6, y + 28), 1, defaultState);
            Checkbox.AddToggleCallback(OnCheckboxChecked);
            
            Slider.SliderCallbacks += OnSliderChange;
        }

        private void OnSliderChange(bool newvalue)
        {
            SliderUpdateEvent?.Invoke(newvalue, Slider.SliderValue);
            Slider.SetTooltip($"{_text}: {Slider.SliderValue:0.0}");
        }

        private void OnCheckboxChecked(object sender, bool status)
        {
            CheckboxUpdateEvent?.Invoke(status);
        }

        public delegate void CheckboxUpdate(bool state);
        public delegate void SliderUpdate(bool dragging, double newValue);
    }
}