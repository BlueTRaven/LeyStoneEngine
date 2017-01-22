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

namespace LeyStoneEngine.Guis.Widgets
{
    public class WidgetButtonToggle : Widget
    {
        public bool on;

        public WidgetButtonToggle(Rectangle bounds, bool startOn) : base(bounds)
        {
            this.on = startOn;
        }

        protected override void OnRelease(BaseMain main)
        {
            base.OnRelease(main);

            on = !on;
        }
    }
}
