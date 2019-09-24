using System;
using Annex.eventmanager;
using Annex.eventmanager.events;
using Annex.eventmanager.imp;
using Annex.gui;
using Annex.placeholder.components;
using Annex.util;
using Microsoft.Xna.Framework.Input;

namespace Annex.mod.mods
{
    public class ModSettings : IMod, IListener
    {
        public string Name => "Test Mod";
        public string Description => "Allows for modding the AR";
        
        public void OnInit()
        {
            EventManager.RegisterListener(this);
        }

        [EventMethod]
        public void OnOptionsInit(OptionsInitializeEvent @event)
        {
//            @event.AddOption("Hello World!!", "Toggle it lol", false, 
//                (sender, args) => Logger.Log("Hello World!!!!"));
        }
        
        [EventMethod]
        public void OnKeyPress(KeyPressEvent @event)
        {
            if(@event.KeysPressed == Keys.J)
                GameHelper.ShowScreen(new GuiMapSettings());
        }
        
    }
}