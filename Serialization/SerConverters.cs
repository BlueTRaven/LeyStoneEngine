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
using LeyStoneEngine.Graphics;

namespace LeyStoneEngine.Serialization
{
    public static class SerConverters
    {
        public static PrimitivePolygon ToPolygon(this SerPrimPolygon polygon)
        {
            List<PrimitiveTriangle> tris = new List<PrimitiveTriangle>();
            foreach (SerPrimTriangle t in polygon.Triangles)
            {
                tris.Add(t.ToTriangle());
            }

            return new PrimitivePolygon(tris.ToArray());
        }

        public static PrimitiveTriangle ToTriangle(this SerPrimTriangle triangle)
        {
            Vector2[] verts = new Vector2[3] { triangle.Point1.ToVector2(), triangle.Point2.ToVector2(), triangle.Point3.ToVector2() };
            Color[] colors = new Color[3] { triangle.Color1.ToColor(), triangle.Color2.ToColor(), triangle.Color3.ToColor() };

            TextureContainer texture = null;
            if (triangle.Texture == null || BaseMain.assets.GetTexture(triangle.Texture.Name) == null)
                texture = new TextureContainer(BaseMain.assets.GetTexture("whitePixel"), 1);
            else texture = new TextureContainer(BaseMain.assets.GetTexture(triangle.Texture.Name), triangle.Texture.Scale.ToVector2());

            return new PrimitiveTriangle(verts, colors, texture);
        }

        public static SerVector ToSerVector(this Vector2 vector)
        {
            SerVector v = new SerVector
            {
                X = vector.X,
                Y = vector.Y
            };

            return v;
        }

        public static SerColor ToSerColor(this Color color)
        {
            SerColor c = new SerColor
            {
                R = color.R,
                G = color.G,
                B = color.B,
                A = color.A
            };

            return c;
        }

        public static Vector2 ToVector2(this SerVector vector)
        {
            if (vector == null)
                return Vector2.Zero;
            return new Vector2(vector.X, vector.Y);
        }

        public static Color ToColor(this SerColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
