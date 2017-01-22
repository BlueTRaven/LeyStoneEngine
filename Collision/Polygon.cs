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
    public class Polygon
    {
        public Vector2 center;
        public Vector2 offset;

        public readonly Node[] vertices;

        public Line[] lines;
        
        public Polygon(int numVerts, int numLines)
        {
            vertices = new Node[numVerts];
            lines = new Line[numLines];
        }

        /// <summary>
        /// Checks - and resolves - collisions with worldlines.
        /// Returns a list of vectors it has resolved upon.
        /// </summary>
        public virtual List<Vector2> CheckResolveCollision(BaseWorld world)
        {
            List<Vector2> resolveNormal = new List<Vector2>();

            foreach (Line worldLine in world.lines)
            {
                int numOverlaps = 0;

                float currentOverlapMagnitude = 0;
                Vector2 currentOverlapAxis = Vector2.Zero;

                bool minSet = false;

                int debug = 0;

                Vector2 wLNormal = worldLine.normal;

                Tuple<float, float> minmaxPolyUseCheck = ProjectToAxis(wLNormal);
                float minPolyUC = minmaxPolyUseCheck.Item1;
                float maxPolyUC = minmaxPolyUseCheck.Item2;

                float wLinePoint = worldLine.ProjectToAxis(wLNormal).Item1;    //these are always going to be the same.

                float checkMagnitude = 0;

                if ((wLinePoint > minPolyUC && wLinePoint < maxPolyUC))
                {
                    checkMagnitude = MathHelper.Min(Math.Abs(Math.Abs(minPolyUC) - Math.Abs(wLinePoint)), Math.Abs(Math.Abs(maxPolyUC) - Math.Abs(wLinePoint)));
                }
                else continue;

                foreach (Line polyLine in lines)
                {
                    debug++;
                    Vector2 currentNormal = polyLine.normal;

                    Tuple<float, float> minmaxPoly = ProjectToAxis(currentNormal);
                    float minPoly = minmaxPoly.Item1;
                    float maxPoly = minmaxPoly.Item2;

                    Tuple<float, float> minmaxWLine = worldLine.ProjectToAxis(currentNormal);
                    float minWLine = minmaxWLine.Item1;
                    float maxWLine = minmaxWLine.Item2;

                    if (((maxWLine > minPoly && maxWLine >= maxPoly) && (minWLine <= minPoly && minWLine < maxPoly)) || ((minWLine > minPoly && minWLine < maxPoly) || (maxWLine >= minPoly && maxWLine <= maxPoly)))
                    {
                        if (checkMagnitude < currentOverlapMagnitude || !minSet)
                        {
                            currentOverlapMagnitude = checkMagnitude;
                            currentOverlapAxis = worldLine.normal;

                            minSet = true;
                        }

                        numOverlaps++;
                    }
                    else break;
                }

                if (numOverlaps >= lines.Count()) //Lines.count should also be = to number of distinct vertices.
                {
                    Move(worldLine.normal * currentOverlapMagnitude);
                    resolveNormal.Add(worldLine.normal);
                }
            }

            return resolveNormal;
        }

        /// <summary>
        /// Projects the polygon to a given vector axis.
        /// Returns a tuple containing the min and max values of the lines's nodes.
        /// </summary>
        /// <param name="axis">The axis to project to.</param>
        /// <returns>A tuple containing the min and max values of the polygon's nodes.</returns>
        public Tuple<float, float> ProjectToAxis(Vector2 axis)
        {
            List<float> projectedVerts = new List<float>();
            foreach (Node vert in vertices)
            {   //Add all the projected verts
                projectedVerts.Add(Vector2.Dot(vert.position, axis));
            }

            float min = projectedVerts.Min();
            float max = projectedVerts.Max();

            return new Tuple<float, float>(min, max);
        }

        public Tuple<float, float> ProjectToAxis(Vector2 axis, out Vector2 min, out Vector2 max)
        {
            min = max = Vector2.Zero;

            float minU = 0, maxU = 0;

            bool first = true;
            foreach (Node vertex in vertices)
            {
                float dot = Vector2.Dot(vertex.position, axis);
                if (first)
                {
                    minU = dot;
                    maxU = dot;

                    min = vertex.position;
                    max = vertex.position;
                }

                if (dot < minU)
                {
                    minU = dot;
                    min = vertex.position;
                }

                if (dot > maxU)
                {
                    maxU = dot;
                    max = vertex.position;
                }
            }

            return new Tuple<float, float>(minU, maxU);
        }

        /// <summary>
        /// Moves in a direction.
        /// </summary>
        public void Move(Vector2 direction)
        {
            foreach (Line line in lines)
            {
                line.Move(direction);
            }

            center += direction;
        }

        public void MoveTo(Vector2 position)
        {
            Vector2[] temp = new Vector2[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                temp[i] = lines[i].middle - center;
            }

            center = position;

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].MoveTo(temp[i] + center);
            }
        }

        public bool Contains(Vector2 testPoint)
        {
            bool contains = false;

            List<Vector2> vertices = new List<Vector2>();
            foreach (Line l in lines)
                foreach (Node n in l.nodes)
                    vertices.Add(new Vector2(n.position.X, n.position.Y));

            vertices = vertices.Distinct().ToList();    //get rid of duplicates. Might not be necessary?

            for (int i = 0, j = vertices.Count - 1; i < vertices.Count; j = i++)
            {
                Vector2 currentPosI = new Vector2(vertices[i].X, vertices[i].Y);
                Vector2 currentPosJ = new Vector2(vertices[j].X, vertices[j].Y);
                if ((currentPosI.Y > testPoint.Y) != (currentPosJ.Y > testPoint.Y) &&
                    (testPoint.X < (currentPosJ.X - currentPosI.X) * (testPoint.Y - currentPosI.Y) / (currentPosJ.Y - currentPosI.Y) + currentPosI.X))
                {
                    contains = !contains;

                    /*if (((currentPosI.Y > testPoint.Y) != (currentPosJ.Y > testPoint.Y)) &&
     (testPoint.X < (currentPosJ.X - currentPosI.X) * (testPoint.Y - currentPosI.Y) / (currentPosJ.Y - currentPosI.Y) + currentPosI.X))*/
                }
            }

            return contains;
        }

        public void DrawDebug(SpriteBatch batch)
        {
            foreach (Line l in lines)
                l.DrawDebug(batch);

            DrawPrimitives.DrawLine(batch, vertices[0].position, center, Color.Black, 1);
        }
    }
}
