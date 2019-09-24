using Annex.eventmanager.imp;

namespace Annex.eventmanager.events
{
    public class ScoreSubmitEvent : IEvent
    {
        public bool Canceled { get; set; } = false;
        
        public void PostFire(object inst)
        {
        }
    }
}