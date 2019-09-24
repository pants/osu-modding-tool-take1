using System;
using System.Collections.Generic;
using System.Drawing.Text;
using Annex.gui.components;
using Annex.mod;
using Annex.mod.mods;
using Annex.placeholder.components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Annex.gui
{
    public class GuiMapSettings : GuiScreenWrapper
    {
        //todo: Make this not a mess
        public GuiMapSettings() : base("Map Setting Customizer", false)
        {
            var startY = 44;
            drawEmptyRect(28, 38, 250, 500, 1);

            var mapMod = Annex.ModManager.GetMod(typeof(MapSettingsMod)) as MapSettingsMod;

            {
                var arStatComp = new ModStatComponent(componentManagerFld, "Approach Rate", 34, startY,
                    mapMod.ApproachRateModEnabled, 0, 12, mapMod.ApproachRateModValue);
                AddCheckbox(arStatComp.Checkbox.ComponentList);
                arStatComp.SliderUpdateEvent += (dragging, value) =>
                {
                    if (!dragging) mapMod.ApproachRateModValue = value;
                };

                arStatComp.CheckboxUpdateEvent += state => mapMod.ApproachRateModEnabled = state;
            }

            {
                var csStatComp = new ModStatComponent(componentManagerFld, "Circle Size", 34, startY + 46,
                    mapMod.CircleSizeModEnabled, 0, 12, mapMod.CircleSizeModValue);
                AddCheckbox(csStatComp.Checkbox.ComponentList);
                csStatComp.SliderUpdateEvent += (dragging, value) =>
                {
                    if (!dragging) mapMod.CircleSizeModValue = value;
                };

                csStatComp.CheckboxUpdateEvent += state => mapMod.CircleSizeModEnabled = state;
            }
            
            {
                var odStatMod = new ModStatComponent(componentManagerFld, "Overall Difficulty", 34, startY + (46 * 2),
                    mapMod.OverallDifficulyModEnabled, 0, 12, mapMod.OverallDifficultyModValue);
                AddCheckbox(odStatMod.Checkbox.ComponentList);
                odStatMod.SliderUpdateEvent += (dragging, value) =>
                {
                    if (!dragging) mapMod.OverallDifficultyModValue = value;
                };

                odStatMod.CheckboxUpdateEvent += state => mapMod.OverallDifficulyModEnabled = state;
            }            
            
            {
                var dtStatMod = new ModStatComponent(componentManagerFld, "DoubleTime Speed", 34, startY + (46 * 3),
                    mapMod.DoubletimeSpeedModEnabled, 1.2, 3.0, mapMod.DoubletimeSpeedModValue);
                AddCheckbox(dtStatMod.Checkbox.ComponentList);
                dtStatMod.SliderUpdateEvent += (dragging, value) =>
                {
                    if (!dragging) mapMod.DoubletimeSpeedModValue = value;
                };

                dtStatMod.CheckboxUpdateEvent += state => mapMod.DoubletimeSpeedModEnabled = state;
            }
        }
    }
}