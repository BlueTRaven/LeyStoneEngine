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

namespace LeyStoneEngine.Guis
{
    public abstract class GuiHud : Gui
    {
        public GuiHud() : base()
        {
            stopsWorldInput = false;
            stopsWorldDraw = false;
        }
    }
}
