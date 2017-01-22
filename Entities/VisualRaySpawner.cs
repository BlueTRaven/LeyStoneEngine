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
    public class VisualRaySpawner : Visual
    {
        List<VisualRay> rays = new List<VisualRay>();

        public readonly Vector2 spawnTimer, spawnCount, durationMinMax, length, breadth;
        public int timer;

        Color color;

        public VisualRaySpawner(Vector2 position, int duration, Vector2 spawnTimer, Vector2 spawnCount, Vector2 length, Vector2 breadth, Vector2 durationMinMax, Color color) : base(position, 3)
        {
            SetDies(duration);

            this.spawnTimer = spawnTimer;
            this.spawnCount = spawnCount;
            this.durationMinMax = durationMinMax;
            this.length = length;
            this.breadth = breadth;

            this.color = color;

            solid = false;
        }

        public override void Update(BaseWorld world)
        {
            base.Update(world);

            foreach (VisualRay ray in rays.ToList())
            {
                ray.Update(world);

                if (ray.dead)
                    rays.Remove(ray);
            }

            timer--;

            if (timer <= 0)
            {
                timer = BaseMain.rand.Next((int)spawnTimer.X, (int)spawnTimer.Y);

                for (int i = 0; i < BaseMain.rand.Next((int)spawnCount.X, (int)spawnCount.Y); i++)
                {
                    rays.Add(new VisualRay(position, BaseMain.rand.Next((int)durationMinMax.X, (int)durationMinMax.Y), 
                        (float)BaseMain.rand.NextDouble(0, 360), BaseMain.rand.Next((int)length.X, (int)length.Y), BaseMain.rand.Next((int)breadth.X, (int)breadth.Y), color));
                }
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (VisualRay ray in rays)
                ray.Draw(batch);
        }
    }
}
