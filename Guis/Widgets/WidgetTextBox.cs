using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LeyStoneEngine.Collision;
using LeyStoneEngine.Utility;
using LeyStoneEngine.Interface;
using LeyStoneEngine.Entities;
using LeyStoneEngine.Graphics;
using LeyStoneEngine.Input;

namespace LeyStoneEngine.Guis.Widgets
{
    public enum AllowedTextType
    {
        Numerical,
        Alphabetical,
        Both
    }

    public class WidgetTextBox : Widget
    {
        private AllowedTextType allowedText;
        private int maxCharCount;

        public StringBuilder text;

        SpriteFont font;
        private string emptyText;
        TextAlignment alignment;

        private bool isFocus;

        public WidgetTextBox(Rectangle bounds, AllowedTextType allowedText, int maxCharCount, SpriteFont font, string emptyText, TextAlignment alignment) : base(bounds)
        {
            this.allowedText = allowedText;
            this.maxCharCount = maxCharCount;

            this.font = font;
            this.emptyText = emptyText;
            this.alignment = alignment;

            text = new StringBuilder();
        }

        public override void Update(BaseMain main)
        {
            base.Update(main);
            if ((BaseMain.mouse.MouseKeyPress(MouseButton.Left) && !Pressed) || BaseMain.keyboard.KeyPressed(Keys.Enter))
            {
                isFocus = false;
            }

            if (isFocus)
            {
                if (BaseMain.keyboard.KeyPressed(Keys.Back))
                {
                    if (text.Length > 0)
                        text.Remove(text.Length - 1, 1);
                }
                else
                {
                    Console.WriteLine("test");
                    char c;
                    BaseMain.keyboard.GetChar(out c);
                    if (text.Length < maxCharCount)
                    {
                        if (allowedText == AllowedTextType.Alphabetical && Char.IsLetter(c))
                            text.Append(c);
                        else if (allowedText == AllowedTextType.Numerical && Char.IsDigit(c))
                            text.Append(c);
                        else if (allowedText == AllowedTextType.Both && Char.IsLetterOrDigit(c))
                            text.Append(c);
                    }
                }
            }
        }

        protected override void OnPress(BaseMain main)
        {
            isFocus = true;
            BaseMain.keyboard.stopInput = true;
        }

        public override void Draw(SpriteBatch batch)
        {
            drawFunc?.Invoke(batch, this);

            Vector2 offset = Vector2.Zero;

            if (text.Length <= 0 && !isFocus)
            {
                offset = TextHelper.GetAlignmentOffset(font, emptyText, bounds, alignment);
                batch.DrawString(font, emptyText, bounds.Location.ToVector2() + offset, Color.Gray);
            }
            else
            {
                offset = TextHelper.GetAlignmentOffset(font, text.ToString(), bounds, alignment);
                batch.DrawString(font, text, bounds.Location.ToVector2() + offset, Color.Black);
            }
        }
    }
}
