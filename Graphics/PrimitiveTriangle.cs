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
    public enum VisualType
    {
        Color,
        Texture
    }

    public class PrimitiveTriangle
    {
        public static bool DEBUGdrawTriangleOutlines = false;
        public VertexPositionColorTexture[] vertices;
        public Vector2[] translatedVertices;
        public Vector2 rotationPos; //note that this is relative to the world's position.

        public bool clockwise = true;

        public TextureContainer textureContainer;

        public Vector2 origin;
        public float rotation;

        public PrimitiveTriangle(Vector2[] vertices, Color[] colors, TextureContainer texture = null)
        {
            this.vertices = new VertexPositionColorTexture[3];

            if (texture == null)
                texture = new TextureContainer(BaseMain.assets.GetTexture("whitePixel"), 1);

            for (int i = 0; i < 3; i++)
            {
                this.vertices[i] = new VertexPositionColorTexture(new Vector3(vertices[i], 0), colors[i], new Vector2((vertices[i].X) / (64 * texture.scale.X), (vertices[i].Y) / (64 * texture.scale.Y)));
            }

            if (!DetermineClockwise())
            {
                Flip();
            }

            this.textureContainer = texture;

            this.origin = Vector2.Zero;

            DetermineOriginVertices();
        }

        public PrimitiveTriangle SetAllColors(Color toColor)
        {
            for (int i = 0; i < 3; i++)
                vertices[i].Color = toColor;

            return this;
        }

        public bool DetermineClockwise()
        {
            List<Vector2> v = new List<Vector2>();

            foreach (VertexPositionColorTexture vc in vertices)
                v.Add(new Vector2(vc.Position.X, vc.Position.Y));

            return VectorHelper.GetPolygonArea(v.ToArray()) < 0;
        }

        public void Flip()
        {
            VertexPositionColorTexture temp = new VertexPositionColorTexture(vertices[0].Position, vertices[0].Color, vertices[0].TextureCoordinate);
            vertices[0] = new VertexPositionColorTexture(vertices[1].Position, vertices[1].Color, vertices[1].TextureCoordinate);
            vertices[1] = temp;
        }

        public bool Contains(Vector2 point)
        {
            float as_x = point.X - vertices[0].Position.X;
            float as_y = point.Y - vertices[0].Position.Y;

            bool s_ab = (vertices[1].Position.X - vertices[0].Position.X) * as_y - (vertices[1].Position.Y - vertices[0].Position.Y) * as_x > 0;

            if ((vertices[2].Position.X - vertices[0].Position.X) * as_y - (vertices[2].Position.Y - vertices[0].Position.Y) * as_x > 0 == s_ab) return false;

            if ((vertices[2].Position.X - vertices[1].Position.X) * (point.Y - vertices[1].Position.Y) - (vertices[2].Position.Y - vertices[1].Position.Y) * (point.X - vertices[1].Position.X) > 0 != s_ab) return false;

            return true;
        }

        public Vector2 GetCenter()
        {
            float centerX = (vertices[0].Position.X + vertices[1].Position.X + vertices[2].Position.X) / 3;
            float centerY = (vertices[0].Position.Y + vertices[1].Position.Y + vertices[2].Position.Y) / 3;

            return new Vector2(centerX, centerY);
        }

        public void DetermineOriginVertices(Vector2 position = new Vector2 { X = 0, Y = 0} )
        {
            translatedVertices = new Vector2[3];

            Vector2 center = GetCenter();

            if (position != Vector2.Zero)
                this.rotationPos = position;            //I set it to this variable, changing the location it will rotate around.
            else this.rotationPos = center;

            for (int i = 0; i < 3; i++)
            {
                translatedVertices[i] = new Vector2(vertices[i].Position.X, vertices[i].Position.Y) - rotationPos;
            }
        }

        public void Draw(SpriteBatch batch, List<EffectInstance> effects)
        {
            foreach (EffectInstance ei in effects)
            {
                batch.End();
                batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, DrawHelper.primitiveSampler, DepthStencilState.Default, null, ei.effectContainer.effect, BaseMain.camera.GetViewMatrix());

                if (ei.effectContainer.use == EffectUse.Pre)
                    ei.Apply();
                if (ei.effectContainer.effect.GetType() == typeof(BasicEffect))
                {
                    ((BasicEffect)ei.effectContainer.effect).Texture = textureContainer.texture;
                }
            }

            VertexPositionColorTexture[] movedVerts = new VertexPositionColorTexture[vertices.Length];

            for (int i = 0; i < translatedVertices.Length; i++)
            {
                movedVerts[i] = new VertexPositionColorTexture(
                    new Vector3(Vector2.Transform(translatedVertices[i], Matrix.CreateRotationZ(MathHelper.ToRadians(rotation))) + rotationPos, 0)
                    , vertices[i].Color, vertices[i].TextureCoordinate);
                //movedVerts[i] = new VertexPositionColorTexture(new Vector3(translatedVertices[i], 0), vertices[i].Color, vertices[i].TextureCoordinate);
            }
            //batch.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertices, 0, vertices.Length, new int[] { 0, 1, 2 }, 0, 1);

            batch.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, movedVerts, 0, vertices.Length, new int[] { 0, 1, 2 }, 0, 1);

            foreach (EffectInstance ei in effects)
            {
                batch.End();
                batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, DrawHelper.primitiveSampler, DepthStencilState.Default, null, ei.effectContainer.effect, BaseMain.camera.GetViewMatrix());

                if (ei.effectContainer.use == EffectUse.Post)
                    ei.Apply();
            }
        }
    }
}
