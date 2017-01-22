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
    public class Line
    {
        public Node[] nodes;

        public Node leftNode, rightNode;

        public Vector2 normal;

        private float length;

        public Vector2 middle;

        private bool flip;

        public static bool DEBUGDrawLines = false;

        public Line(Node leftNode, Node rightNode, bool reverse = false)
        {
            nodes = new Node[2];

            nodes[0] = leftNode;
            nodes[1] = rightNode;

            this.leftNode = leftNode;
            this.rightNode = rightNode;

            normal = VectorHelper.GetPerpendicular(rightNode.position - leftNode.position, reverse);
            flip = reverse;

            length = (leftNode.position - rightNode.position).Length();

            middle = leftNode.position + (Vector2.Normalize((rightNode.position - leftNode.position)) * (length / 2));
        }

        /// <summary>
        /// Tests to see if a line is overlapping another.
        /// Using SAT collision detection
        /// </summary>
        public bool CheckOverlapping(Line givenLine, out float overlapMagnitude)
        {
            float projThis = Vector2.Dot(leftNode.position, normal);

            float projGivenLeft = Vector2.Dot(givenLine.leftNode.position, normal);
            float projGivenRight = Vector2.Dot(givenLine.rightNode.position, normal);
            float[] dots2 = new float[] { projGivenLeft, projGivenRight };

            float maxGivenLine = dots2.Max();
            float minGivenLine = dots2.Min();

            if (projThis < maxGivenLine && projThis > minGivenLine)
            {
                overlapMagnitude = MathHelper.Min(Math.Abs(projThis - maxGivenLine), Math.Abs(minGivenLine - projThis));
                return true;
            }
            else
            {
                overlapMagnitude = 0;
                return false;
            }
        }

        /// <summary>
        /// Projects the line to a given vector axis.
        /// Returns a tuple containing the min and max values of the lines's nodes.
        /// </summary>
        /// <param name="axis">The axis to project to.</param>
        /// <returns>A tuple containing the min and max values of the lines's nodes.</returns>
        public Tuple<float, float> ProjectToAxis(Vector2 axis)
        {
            List<float> projectedWorldLineVerts = new List<float>();

            projectedWorldLineVerts.Add(Vector2.Dot(leftNode.position, axis));
            projectedWorldLineVerts.Add(Vector2.Dot(rightNode.position, axis));

            float min = projectedWorldLineVerts.Min();
            float max = projectedWorldLineVerts.Max();

            return new Tuple<float, float>(min, max);
        }

        public void Flip()
        {
            flip = !flip;
            normal = VectorHelper.GetPerpendicular(rightNode.position - leftNode.position, flip);
        }

        public void Move(Vector2 direction)
        {
            foreach(Node n in nodes)
                n.position += direction;

            middle += direction;
        }

        public void MoveTo(Vector2 vecTo)
        {
            Vector2 distVecLeft = leftNode.position - middle;   //get the distance between the middle and the vector
            Vector2 distVecRight = rightNode.position - middle;

            middle = vecTo;

            leftNode.position = distVecLeft + middle;   //move it to the new middle + the distance vector
            rightNode.position = distVecRight + middle;
        }

        public void DrawDebug(SpriteBatch batch)
        {
            if (DEBUGDrawLines)
            {
                leftNode.DrawDebug(batch);
                rightNode.DrawDebug(batch);
                DrawPrimitives.DrawLine(batch, leftNode.position, rightNode.position, Color.Black, 1);

                DrawPrimitives.DrawRectangle(batch, new Rectangle((middle - new Vector2(1)).ToPoint(), new Point(2)), Color.Red);

                DrawPrimitives.DrawLine(batch, middle, middle + normal * 48, Color.Gray, 1);
            }
        }
    }
}
