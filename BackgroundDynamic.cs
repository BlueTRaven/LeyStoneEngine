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

namespace LeyStoneEngine
{
    public class BackgroundDynamic : Background
    {
        public PrimitivePolygon backPlate;

        List<PrimitivePolygon> originPolygons = new List<PrimitivePolygon>();

        public BackgroundDynamic(Texture2D texture, Vector2 scrollRate, List<PrimitivePolygon> polygons, Action<SpriteBatch, Background> drawFunc, List<EffectInstance> effects = null) : base(texture, scrollRate, polygons, drawFunc, effects)
        {
            this.originPolygons = polygons;
        }

        public void SetPolygons()
        {
            for (int k = 0; k < polygons.Count; k++)
            {
                for (int i = 0; i < polygons[k].triangles.Count; i++)
                {
                    for (int j = 0; j < polygons[k].triangles[i].vertices.Length; j++)
                    {
                        this.polygons[k].triangles[i].vertices[j].Position = new Vector3(-BaseMain.camera.Position, 0) + this.originPolygons[k].triangles[i].vertices[j].Position;
                    }
                }
            }
        }

        public override void Draw(BaseWorld world, SpriteBatch batch)
        {
            SetPolygons();
            //DrawRenderTarget(batch);
            //counter = 0;
            base.Draw(world, batch);
        }
    }
}
