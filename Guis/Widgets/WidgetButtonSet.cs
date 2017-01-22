using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LeyStoneEngine.Collision;
using LeyStoneEngine.Utility;
using LeyStoneEngine.Interface;
using LeyStoneEngine.Entities;
using LeyStoneEngine.Guis.Widgets;

namespace LeyStoneEngine.Guis.Widgets
{
    public class WidgetButtonSet : Widget
    {
        public readonly List<WidgetButton> buttons = new List<WidgetButton>();

        int currentPressedIndex = 0;

        public WidgetButtonSet(Rectangle bounds, params WidgetButton[] buttons) : base(bounds)
        {
            this.buttons = buttons.ToList();

            foreach (WidgetButton button in this.buttons)
            {
                button.bounds = new Rectangle(button.bounds.Location + bounds.Location, button.bounds.Size);
            }
        }

        public override void Update(BaseMain main)
        {
            base.Update(main);

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Update(main);
                if (buttons[i].Pressed)
                {
                    currentPressedIndex = i;
                }
            }

            if (currentPressedIndex >= 0)
            {
                buttons[currentPressedIndex].state = ClickableState.Pressed;
            }
        }
    }
}
