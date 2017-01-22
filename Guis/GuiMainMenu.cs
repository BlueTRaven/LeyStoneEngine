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
    public class GuiMainMenu : Gui
    {
        private readonly BaseWorld world;
        public GuiMainMenu(GuiHud hud, BaseWorld world) : base()
        {
            WidgetButton button = new WidgetButton(new Rectangle(Point.Zero, new Point(128, 32)));
            button.drawFunc = new Action<SpriteBatch, Widget>((batch, widget) =>
            {
                DrawPrimitives.DrawRectangle(batch, button.bounds, Color.White);

                Color color = Color.LightGray;
                if (button.Hovered)
                    color = Color.DarkGray;
                else if (button.Pressed)
                    color = Color.Gray;
                DrawPrimitives.DrawRectangle(batch, new Rectangle(button.bounds.Location + new Point(2), button.bounds.Size - new Point(4)), color);
            });
            button.releaseFunc = new Action<BaseMain, Widget>((main, widget) =>
            {
                BaseMain.currentGui = hud;

                main.world = world;
                world.Start();
            });

            widgets.Add(button);

            stopsWorldInput = true;
            stopsWorldDraw = true;

            this.world = world;
        }

        public override void Update(BaseMain main)
        {
            for (int i = 0; i < widgets.Count; i++)
            {
                if (i == 0)
                    if (BaseMain.keyboard.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
                        widgets[i].releaseOverride = true;
                widgets[i].Update(main);
            }
        }

        public override void PostUpdate()
        {
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.GraphicsDevice.Clear(Color.Black);
            foreach (Widget w in widgets)
            {
                w.Draw(batch);
            }
        }
    }
}
