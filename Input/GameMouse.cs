using System;
using System.Threading;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace LeyStoneEngine.Input
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    public class GameMouse
    {
        public MouseState currentState;
        public MouseState prevState;

        /// <summary>
        /// Returns the mouse's position in camera space.
        /// </summary>
        public Vector2 position { get { return currentState.Position.ToVector2(); } }
        //public Item heldItem;
        //public ItemSlot hoveredSlot;

        public Vector2 lastClickPos;

        private SoundEffectInstance errorNoise;
        public GameMouse() { }

        public void Update()
        {
            currentState = Mouse.GetState();

            if (MouseKeyPress(MouseButton.Left))
                lastClickPos = currentState.Position.ToVector2();
        }

        public void PostUpdate()
        {
            prevState = currentState;
        }

        public bool MouseKeyPress(MouseButton button)
        {
            if (button == MouseButton.Left)
                return currentState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released;
            else return false;
        }

        public bool MouseKeyPressContinuous(MouseButton button)
        {
            if (button == MouseButton.Left)
                return BaseMain.mouse.currentState.LeftButton == ButtonState.Pressed;
            else return false;
        }

        public void Draw(SpriteBatch batch)
        {
        }
    }
}
