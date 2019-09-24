using System;
using Annex.eventmanager;
using Annex.eventmanager.events;
using Annex.eventmanager.imp;

namespace Annex.mod.mods
{
    public class MapSettingsMod : IMod, IListener
    {
        public string Name => "Map Settings";
        public string Description => "Handles Map Settings such as Approach Rate and Circle Size.";

        public bool ApproachRateModEnabled { get; set; } = false;
        public bool CircleSizeModEnabled { get; set; } = false;
        public bool OverallDifficulyModEnabled { get; set; } = false;
        public bool DoubletimeSpeedModEnabled { get; set; } = false;

        public double ApproachRateModValue { get; set; } = 8.0;
        public double CircleSizeModValue { get; set; } = 5.0;
        public double OverallDifficultyModValue { get; set; } = 10.0;
        public double DoubletimeSpeedModValue { get; set; } = 1.5;

        public void OnInit()
        {
            EventManager.RegisterListener(this);
        }

        [EventMethod]
        public void OnScoreSubmit(ScoreSubmitEvent @event)
        {
            @event.Canceled = ApproachRateModEnabled || CircleSizeModEnabled || OverallDifficulyModEnabled ||
                               DoubletimeSpeedModEnabled;
        }

        [EventMethod]
        public void OnMapUpdate(MapSettingEvent @event)
        {
            switch (@event.Setting)
            {
                case MapSettings.ApproachRate:
                    @event.Value = (float)ApproachRateModValue;
                    @event.Canceled = !ApproachRateModEnabled;
                    break;
                case MapSettings.CircleSize:
                    @event.Value = (float)CircleSizeModValue;
                    @event.Canceled = !CircleSizeModEnabled;
                    break;
                case MapSettings.OverallDifficulty:
                    @event.Value = (float) OverallDifficultyModValue;
                    @event.Canceled = !OverallDifficulyModEnabled;
                    break;
                case MapSettings.DoubletimeSpeed:
                    @event.Canceled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}