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

namespace LeyStoneEngine.Graphics
{
    public class PrimitivePolygon
    {
        public readonly List<PrimitiveTriangle> triangles = new List<PrimitiveTriangle>();

        public PrimitivePolygon(Vector2[] vertices, Color[] colors, TextureContainer texture = null)
        {
            CreateTriangles(vertices, colors, texture);
        }
        public PrimitivePolygon(Vector2[] vertices, Color color, TextureContainer texture = null)
        {
            Color[] colors = new Color[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
                colors[i] = color;

            CreateTriangles(vertices, colors, texture);
        }

        public PrimitivePolygon(PrimitiveTriangle[] triangles)
        {
            this.triangles = triangles.ToList();

            foreach (PrimitiveTriangle t in this.triangles)
                if (!t.DetermineClockwise())
                    t.Flip();
        }

        public PrimitivePolygon(Vector2 center, float radius, int sides, Color color, TextureContainer texture = null)
        {
            Vector2[] vertices = new Vector2[sides];

            Vector2 prevPos = new Vector2(-1, 0) * radius;
            for (float i = 1; i <= sides; i++)
            {
                Vector2 currentPos = Vector2.Normalize(Vector2.Transform(new Vector2(-1, 0), Matrix.CreateRotationZ(MathHelper.ToRadians((360 / (float)sides) * i)))) * radius;

                vertices[(int)i - 1] = currentPos + center;
            }

            CreateTriangles(vertices, color, texture);
        }

        public PrimitivePolygon(Vector2 center, float spineWidth, float spineHeight, Color color, int dummy, TextureContainer texture = null)
        {
            List<Vector2> vertices = new List<Vector2>();

            vertices.Add(center);
            vertices.Add(center + new Vector2(spineWidth / 2, -spineHeight));
            vertices.Add(center + new Vector2(spineWidth, 0));

            CreateTriangles(vertices.ToArray(), color, texture);
            vertices.Clear();

            vertices.Add(center + new Vector2(spineWidth, 0));
            vertices.Add(center + new Vector2(spineHeight + spineWidth, spineWidth / 2));
            vertices.Add(center + new Vector2(spineWidth, spineWidth));

            CreateTriangles(vertices.ToArray(), color, texture);
            vertices.Clear();

            vertices.Add(center + new Vector2(0, spineWidth));
            vertices.Add(center + new Vector2(spineWidth / 2, spineHeight + spineWidth));
            vertices.Add(center + new Vector2(spineWidth, spineWidth));

            CreateTriangles(vertices.ToArray(), color, texture);
            vertices.Clear();

            vertices.Add(center + new Vector2(0, spineWidth));
            vertices.Add(center + new Vector2(-spineHeight, spineWidth / 2));
            vertices.Add(center);

            CreateTriangles(vertices.ToArray(), color, texture);
            vertices.Clear();

            vertices.Add(center);
            vertices.Add(center + new Vector2(spineWidth, 0));
            vertices.Add(center + new Vector2(spineWidth, spineWidth));
            vertices.Add(center + new Vector2(0, spineWidth));

            CreateTriangles(vertices.ToArray(), color, texture);
        }

        public PrimitivePolygon SetRandomColors(Color color1, Color color2)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                int  r = BaseMain.rand.Next(Math.Min(color1.R, color2.R), Math.Max(color1.R, color2.R));
                int g = BaseMain.rand.Next(Math.Min(color1.G, color2.G), Math.Max(color1.G, color2.G));
                int b = BaseMain.rand.Next(Math.Min(color1.B, color2.B), Math.Max(color1.B, color2.B));
                int a = BaseMain.rand.Next(Math.Min(color1.A, color2.A), Math.Max(color1.A, color2.A));

                Color color = new Color(r, g, b, a);
                triangles[i].SetAllColors(color);
            }

            return this;
        }

        private void CreateTriangles(Vector2[] vertices, Color[] colors, TextureContainer texture)
        {
            for (int i = 1; i <= vertices.Length - 2; i++)
            {
                Vector2[] verts = new Vector2[3] { vertices[0], vertices[i], vertices[i + 1] };
                Color[] cs = new Color[3] { colors[0], colors[i], colors[i + 1]};

                triangles.Add(new PrimitiveTriangle(verts, cs, texture));
            }
        }

        private void CreateTriangles(Vector2[] vertices, Color color, TextureContainer texture)
        {
            for (int i = 1; i <= vertices.Length - 2; i++)
            {
                Vector2[] verts = new Vector2[3] { vertices[0], vertices[i], vertices[i + 1] };
                Color[] cs = new Color[3] { color, color, color };

                triangles.Add(new PrimitiveTriangle(verts, cs, texture));
            }
        }

        public void SetRotation(Vector2 aboutPos, float setRotation)
        {
            foreach(PrimitiveTriangle triangle in triangles)
            {
                //if (triangle.rotationPos != aboutPos)
                {
                    triangle.DetermineOriginVertices(aboutPos);
                    //triangle.origin = aboutPos;
                }

                triangle.rotation = setRotation;
            }
        }

        public void Rotate(Vector2 aboutPos, float rotateAmount)
        {
            foreach (PrimitiveTriangle triangle in triangles)
            {
                //if (triangle.rotationPos != aboutPos)
                {
                    triangle.DetermineOriginVertices(aboutPos);
                    //triangle.origin = aboutPos;
                }

                triangle.rotation += rotateAmount;
            }
        }

        public void ProjectToAxis(Vector2 axis, out Vector2 min, out Vector2 max)
        {
            min = max = Vector2.Zero;

            float minU = 0, maxU = 0;

            bool first = true;

            for (int i = 0; i < triangles.Count; i++)
            {
                for (int j = 0; j < triangles[i].vertices.Length; j++)
                {
                    Vector2 vertexPos = new Vector2(triangles[i].vertices[j].Position.X, triangles[i].vertices[j].Position.Y);
                    float dot = Vector2.Dot(vertexPos, axis);
                    if (first)
                    {
                        minU = dot;
                        maxU = dot;

                        min = vertexPos;
                        max = vertexPos;
                    }

                    if (dot < minU)
                    {
                        minU = dot;
                        min = vertexPos;
                    }

                    if (dot >= maxU)
                    {
                        maxU = dot;
                        max = vertexPos;
                    }
                }
            }
        }

        public void Flip()
        {
            foreach (PrimitiveTriangle t in triangles)
                t.Flip();
        }

        public bool Contains(Vector2 testPoint)
        {
            bool contains = false;

            List<Vector2> verticesColor = new List<Vector2>();
            foreach (PrimitiveTriangle t in triangles)
                foreach (VertexPositionColorTexture vc in t.vertices)
                    verticesColor.Add(new Vector2(vc.Position.X, vc.Position.Y));

            for (int i = 0, j = verticesColor.Count - 1; i < verticesColor.Count; j = i++)
            {
                Vector2 currentPosI = new Vector2(verticesColor[i].X, verticesColor[i].Y);
                Vector2 currentPosJ = new Vector2(verticesColor[j].X, verticesColor[j].Y);
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

        public void Draw(SpriteBatch batch, List<EffectInstance> effects)
        {
            foreach (PrimitiveTriangle t in triangles)
            {
                t.Draw(batch, effects);
            }
        }

        public PrimitivePolygon Copy()
        {
            List<Vector2> vertices = new List<Vector2>();
            List<Color> colors = new List<Color>();

            foreach (PrimitiveTriangle tri in triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    vertices.Add(new Vector2(tri.vertices[i].Position.X, tri.vertices[i].Position.Y));
                    colors.Add(tri.vertices[i].Color);
                }
            }

            return new PrimitivePolygon(vertices.ToArray(), colors.ToArray(), triangles[0].textureContainer);
        }
    }
}
