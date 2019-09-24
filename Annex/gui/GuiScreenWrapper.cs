using System.Collections.Generic;
using Annex.placeholder.components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Annex.gui
{
    public class GuiScreenWrapper : GuiScreen
    {
        public void drawEmptyRect(float x, float y, float w, float h, int borderWidth)
        {
            AddItem(new CompRect(new Vector2(x, y), new Vector2(w, h), 0.90f, Color.White));
            AddItem(new CompRect(new Vector2(x + borderWidth, y + borderWidth),
                new Vector2(w - (borderWidth * 2), h - (borderWidth * 2)),
                0.91f, Color.Black));
        }

        public void AddItem(object o)
        {
            componentManagerFld.AddComponent(o);
        }

        public void AddCheckbox(List<object> o)
        {
            o.ForEach(AddItem);
        }

        protected GuiScreenWrapper(string title, bool boolean) : base(title, boolean)
        {
        }
    }
}