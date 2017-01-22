using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LeyStoneEngine;
using LeyStoneEngine.Utility;
using LeyStoneEngine.Interface;

namespace LeyStoneEngine.Collision
{
    public class Node
    {
        public Vector2 position;

        public Node(Vector2 position)
        {
            this.position = position;
        }

        public void DrawDebug(SpriteBatch batch)
        {
            DrawPrimitives.DrawRectangle(batch, new Rectangle((position - new Vector2(1)).ToPoint(), new Point(2)), Color.Black);
            DrawPrimitives.DrawCircle(batch, position, 6, Color.Black, 1, 8);
        }
    }
}
