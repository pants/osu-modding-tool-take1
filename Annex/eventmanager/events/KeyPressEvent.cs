using Annex.eventmanager.imp;
using Microsoft.Xna.Framework.Input;

namespace Annex.eventmanager.events
{
    public class KeyPressEvent : IEvent
    {
        public readonly Keys KeysPressed;

        public KeyPressEvent(Keys keys)
        {
            KeysPressed = keys;
        }
        
        public void PostFire(object inst)
        {
           // throw new System.NotImplementedException();
        }
    }
}