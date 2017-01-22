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
    public class VisualRay : Visual
    {
        public readonly PrimitivePolygon ray;

        public readonly float angle, length, breadth;

        public readonly Color color;

        /// <summary>
        /// Creates a new instance of a Ray.
        /// </summary>
        /// <param name="duration">How long the ray will last.</param>
        /// <param name="angle">The angle to spawn at.</param>
        /// <param name="length">How long the ray is.</param>
        /// <param name="breadth">How 'wide,' or how far of an angle between the sides of the ray</param>
        public VisualRay(Vector2 position, int duration, float angle, float length, float breadth, Color color) : base(position, 2)
        {
            SetDies(duration);

            this.angle = angle;
            this.length = length;
            this.breadth = breadth;

            this.color = color;

            Vector2 ray1 = position + (Vector2.Transform(new Vector2(-1, 0), Matrix.CreateRotationZ(MathHelper.ToRadians(angle + breadth))) * length);
            Vector2 ray2 = position + (Vector2.Transform(new Vector2(-1, 0), Matrix.CreateRotationZ(MathHelper.ToRadians(angle - breadth))) * length);
            ray = new PrimitivePolygon(new Vector2[] { position, ray1, ray2 }, new Color[] { color, color, color} );

            solid = false;
        }

        public override void Draw(SpriteBatch batch)
        {
            ray.Draw(batch, new EffectInstance[] { new EffectInstance(BaseMain.assets.GetEffectContainer("basicEffect")) }.ToList());
        }
    }
}
