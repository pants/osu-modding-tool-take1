using System;
using System.Collections.Generic;
using Annex.eventmanager.imp;
using Annex.placeholder.components;

namespace Annex.eventmanager.events
{
    public class OptionsInitializeEvent : IEvent
    {
        private List<object> Options { get; }

        public OptionsInitializeEvent()
        {
            Options = new List<object>();
        }

        public void AddOption(string text, string desc, bool enabled, EventHandler toggleMethod)
        {
            Options.Add(new OptionsCheckbox(text, desc, new BoolObj(enabled), toggleMethod));
        }
        
        public void PostFire(object inst)
        {
            var options = inst as GuiOptions;
            
            Options.ForEach(o => options.AddOption(o as OptionsCheckbox));
        }
        //Messenger
    }
}