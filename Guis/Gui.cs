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
    public abstract class Gui
    {
        public bool active = true;
        public bool dead;
        public bool stopsWorldDraw = true, stopsWorldInput = true;

        public List<Widget> widgets = new List<Widget>();

        public TimeScale timeScale;

        public Gui()
        {
            timeScale = new TimeScale(1);
        }

        public abstract void Update(BaseMain main);
        public abstract void PostUpdate();

        public abstract void Draw(SpriteBatch batch);
    }
}
