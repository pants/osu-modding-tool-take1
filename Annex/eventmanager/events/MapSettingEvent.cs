using Annex.eventmanager.imp;

namespace Annex.eventmanager.events
{
    public enum MapSettings
    {
        ApproachRate, OverallDifficulty, CircleSize, DoubletimeSpeed
    }
    
    public class MapSettingEvent : IEvent
    {
        public MapSettings Setting { get; }
        public float Value { get; set; }
        public float DefaultValue { get; }

        public bool Canceled { get; set; } = false;

        public MapSettingEvent(MapSettings setting, float value)
        {
            Setting = setting;
            DefaultValue = value;
        }

        public void PostFire(object inst)
        {
        }
    }
}