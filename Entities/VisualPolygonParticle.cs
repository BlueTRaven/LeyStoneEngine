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

namespace LeyStoneEngine.Entities
{
    public class VisualPolygonParticle : Visual
    {
        private readonly PrimitivePolygon polygon, dummyPoly;

        private bool setTris = false;

        public VisualPolygonParticle(Vector2 position, PrimitivePolygon polygon, float speed) : base(position, 1)
        {
            velocity = Vector2.Transform(new Vector2(-1, 0), Matrix.CreateRotationZ(MathHelper.ToRadians((float)BaseMain.rand.NextDouble(0, 360)))) * speed;

            this.position = hitpoly.center;

            this.dummyPoly = polygon.Copy();
            this.polygon = polygon.Copy();

            this.baseColor = polygon.triangles[0].vertices[0].Color;
            this.currentColor = polygon.triangles[0].vertices[0].Color;
            //this.triangle = new PrimitiveTriangle(points, new Color[] { color, color, color });

            solid = false;
        }

        public override void Update(BaseWorld world)
        {
            base.Update(world);

            if (delay < 0)
            {
                Move();

                for (int i = 0; i < polygon.triangles.Count; i++)
                {
                    for (int j = 0; j < polygon.triangles[i].vertices.Length; j++)
                    {
                        this.polygon.triangles[i].vertices[j].Position = new Vector3(position, 0) + this.dummyPoly.triangles[i].vertices[j].Position;
                    }
                }

                if (!setTris)
                {
                    setTris = true;
                }

                polygon.Rotate(position + offset, spinSpeed);

                foreach (PrimitiveTriangle triangle in polygon.triangles)
                {
                    triangle.SetAllColors(currentColor);
                }
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (alive >= 1)
            polygon.Draw(batch, new EffectInstance[] { new EffectInstance(BaseMain.assets.GetEffectContainer("basicEffect")) }.ToList());
            //hitbox.DrawDebug(batch);
            //DrawPrimitives.DrawCircle(batch, position + offset, 32, Color.Blue);
        }
    }
}
