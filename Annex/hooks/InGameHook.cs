using Annex.eventmanager;
using Annex.eventmanager.events;

namespace Annex.hooks
{
    public class InGameHook
    {
        public static bool ShouldAllowSubmission()
        {
            var _event = new ScoreSubmitEvent();
            EventManager.Invoke(_event);
            return !_event.Canceled;
        }
    }
}