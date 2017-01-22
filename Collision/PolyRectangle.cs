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
    public class PolyRectangle : Polygon
    {
        public PolyRectangle(Vector2 point1, Vector2 point2) : base(8, 4)
        {
            vertices[0] = point1.ToNode();                              //Line 1
            vertices[1] = new Node(new Vector2(point2.X, point1.Y));    //}
            vertices[2] = new Node(new Vector2(point2.X, point1.Y));    //Line 2
            vertices[3] = point2.ToNode();                              //}
            vertices[4] = point2.ToNode();                              //Line 3
            vertices[5] = new Node(new Vector2(point1.X, point2.Y));    //}
            vertices[6] = new Node(new Vector2(point1.X, point2.Y));    //Line 4
            vertices[7] = point1.ToNode();                              //}

            lines[0] = new Line(vertices[0], vertices[1], true);
            lines[1] = new Line(vertices[2], vertices[3], true);
            lines[2] = new Line(vertices[4], vertices[5], true);
            lines[3] = new Line(vertices[6], vertices[7], true);

            center = ((point2 - point1) / 2) + point1;
        }

        /// <summary>
        /// Moves TO a point. NOTE: Relative to the center of all the vertices.
        /// </summary>
        public void MoveTo(Vector2 toPos)
        {
            Vector2[] temp = new Vector2[4];

            temp[0] = lines[0].middle - center;
            temp[1] = lines[1].middle - center;
            temp[2] = lines[2].middle - center;
            temp[3] = lines[3].middle - center;

            center = toPos;

            lines[0].MoveTo(center + temp[0]);
            lines[1].MoveTo(center + temp[1]);
            lines[2].MoveTo(center + temp[2]);
            lines[3].MoveTo(center + temp[3]);
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(lines[0].leftNode.position.ToPoint(), (lines[1].rightNode.position - lines[0].leftNode.position).ToPoint());
        }

        public void DrawDebug(SpriteBatch batch)
        {
            foreach (Line l in lines)
                l.DrawDebug(batch);

            DrawPrimitives.DrawLine(batch, vertices[0].position, center, Color.Black, 1);
        }
    }
}
