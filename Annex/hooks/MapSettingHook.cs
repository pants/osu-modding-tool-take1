using Annex.eventmanager;
using Annex.eventmanager.events;

namespace Annex.hooks
{
    /// <summary>
    /// Each of the public methods is called by osu!
    /// It's a lot easier to just put this shit in a static class than to write up the IL to be injected for enums & event handling
    /// </summary>
    public class MapSettingHook
    {
        public static float GetApproachRate(float currentAr) =>
            GetWithEvent(MapSettings.ApproachRate, currentAr);

        public static float GetCircleSize(float currentCs) =>
            GetWithEvent(MapSettings.CircleSize, currentCs);

        public static float GetOverallDifficulty(float currentOd) =>
            GetWithEvent(MapSettings.OverallDifficulty, currentOd);

        public static float GetDoubletimeSpeed(float currentSpeed) =>
            GetWithEvent(MapSettings.DoubletimeSpeed, currentSpeed);

        
        
        
        private static float GetWithEvent(MapSettings setting, float value)
        {
            var _event = new MapSettingEvent(setting, value);
            EventManager.Invoke(_event);

            return _event.Canceled ? value : _event.Value;
        }

        public bool ShouldSubmit()
        {
            var _event = new ScoreSubmitEvent();
            EventManager.Invoke(_event);
            return !_event.Canceled;
        }
    }
}