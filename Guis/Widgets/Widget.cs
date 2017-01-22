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
    public enum ClickableState
    {
        Hovered,
        Pressed,
        Released,
        Unhovered
    }

    public abstract class Widget
    {
        public ClickableState state;

        public Rectangle bounds;

        public Action<SpriteBatch, Widget> drawFunc;
        public Action<BaseMain, Widget> pressFunc, releaseFunc;

        private bool prevHeld = false, setThisFrame = false;

        public bool hoverOverride, pressOverride, releaseOverride, unhoverOverride;
        public bool Hovered { get { return state == ClickableState.Hovered; } set { } }
        public bool Pressed { get { return state == ClickableState.Pressed; } set { } }
        public bool Released { get { return state == ClickableState.Released; } set { } }
        public bool Unhovered { get { return state == ClickableState.Unhovered; } set { } }

        public Widget(Rectangle bounds)
        {
            this.bounds = bounds;
        }

        public virtual void Update(BaseMain main)
        {
            if (bounds.Contains(BaseMain.mouse.position))
            {
                if (prevHeld && !BaseMain.mouse.MouseKeyPressContinuous(Input.MouseButton.Left))
                {
                    OnRelease(main);
                }

                if (BaseMain.mouse.MouseKeyPressContinuous(Input.MouseButton.Left))
                {
                    OnPress(main);
                }
                else
                {
                    OnHover();
                }
            }
            else
            {     
                OnUnhover();
            }

            if (unhoverOverride || releaseOverride || pressOverride || hoverOverride)
            {
                if (unhoverOverride)
                    OnUnhover();
                else if (releaseOverride)
                    OnRelease(main);
                else if (pressOverride)
                    OnPress(main);
                else if (hoverOverride)
                    OnHover();
            }

            hoverOverride = pressOverride = releaseOverride = hoverOverride = false;
        }

        protected virtual void OnHover()
        {
            prevHeld = false;
            state = ClickableState.Hovered;
        }

        protected virtual void OnPress(BaseMain main)
        {
            prevHeld = true;
            state = ClickableState.Pressed;
            pressFunc?.Invoke(main, this);
        }

        protected virtual void OnRelease(BaseMain main)
        {
            prevHeld = false;
            state = ClickableState.Released;
            releaseFunc?.Invoke(main, this);
        }

        protected virtual void OnUnhover()
        {
            state = ClickableState.Unhovered;
        }

        public virtual void Draw(SpriteBatch batch)
        {
            drawFunc?.Invoke(batch, this);
        }
    }
}
