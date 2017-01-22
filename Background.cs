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
    public class Background
    {
        public Texture2D texture;

        public List<List<Vector2>> basePos = new List<List<Vector2>>();
        public List<PrimitivePolygon> polygons = new List<PrimitivePolygon>();
        public Action<SpriteBatch, Background> drawFunc;

        private List<EffectInstance> effectInstances = new List<EffectInstance>();
        public RenderTarget2D renderTarget, renderFull;

        public int counter = 0;

        private Vector2 size = new Vector2 { X = BaseMain.WIDTH, Y = BaseMain.HEIGHT };

        public readonly Vector2 scrollRate; //I use a vector2 so I can have different scrolling rates for x and y
        /// <summary>
        /// Create a background instance.
        /// </summary>
        /// <param name="scrollRate">the rate at which the background will scroll along each axis.</param>
        public Background(Texture2D texture, Vector2 scrollRate, List<PrimitivePolygon> polygons, Action<SpriteBatch, Background> drawFunc, List<EffectInstance> effects = null)
        {
            this.texture = texture;
            this.scrollRate = scrollRate;
            this.polygons = polygons;

            foreach (PrimitivePolygon poly in polygons)
            {
                List<Vector2> list = new List<Vector2>();
                
                foreach (PrimitiveTriangle t in poly.triangles)
                    foreach (VertexPositionColorTexture v in t.vertices)
                        list.Add(new Vector2(v.Position.X, v.Position.Y)); 

                basePos.Add(list);
            }

            this.drawFunc = drawFunc;

            if (effects == null || effects.Count == 0)
            {
                if (effects == null)
                    effects = new List<EffectInstance>();

                effects.Add(new EffectInstance(BaseMain.assets.GetEffectContainer("basicEffect")));
            }
            else
                this.effectInstances = effects;
        }

        public Background(Texture2D texture, Vector2 scrollRate, ref List<PrimitivePolygon> polygons, Action<SpriteBatch, Background> drawFunc, List<EffectInstance> effects = null)
        {
            this.texture = texture;
            this.scrollRate = scrollRate;
            this.polygons = polygons;

            foreach (PrimitivePolygon poly in polygons)
            {
                List<Vector2> list = new List<Vector2>();

                foreach (PrimitiveTriangle t in poly.triangles)
                    foreach (VertexPositionColorTexture v in t.vertices)
                        list.Add(new Vector2(v.Position.X, v.Position.Y));

                basePos.Add(list);
            }

            this.drawFunc = drawFunc;

            if (effects == null || effects.Count == 0)
            {
                if (effects == null)
                    effects = new List<EffectInstance>();

                effects.Add(new EffectInstance(BaseMain.assets.GetEffectContainer("basicEffect")));
            }
            else
                this.effectInstances = effects;
        }

        public Background SetSize(Vector2 size)
        {
            this.size = size;
            return this;
        }

        public Background SetEffectInstances(List<EffectInstance> instances)
        {
            this.effectInstances = instances;

            return this;
        }

        public void DrawRenderTarget(SpriteBatch batch)
        {
            counter++;
            batch.End();

            renderTarget = new RenderTarget2D(batch.GraphicsDevice, (int)size.X, (int)size.Y);
            renderFull = new RenderTarget2D(batch.GraphicsDevice, BaseMain.WIDTH, BaseMain.HEIGHT);

            batch.GraphicsDevice.SetRenderTarget(renderTarget);
            batch.GraphicsDevice.Clear(Color.Transparent);

            DrawHelper.StartDrawWorldSpace(batch, BlendState.AlphaBlend);

            //if (effectInstances == null || effectInstances.Count == 0) effectInstances.Add();

            foreach (EffectInstance ei in effectInstances)
            {
                if (ei.effectContainer.use == EffectUse.Pre)
                {
                    ei.Apply();
                }
            }

            foreach (PrimitivePolygon poly in polygons)
            {
                poly.Draw(batch, new EffectInstance[] { new EffectInstance(BaseMain.assets.GetEffectContainer("basicEffect")) }.ToList());
            }
            drawFunc?.Invoke(batch, this);

            foreach (EffectInstance ei in effectInstances)
            {
                if (ei.effectContainer.use == EffectUse.Post)
                {
                    ei.Apply();
                }
            }

            batch.End();

            batch.GraphicsDevice.SetRenderTarget(null);

            batch.GraphicsDevice.Clear(Color.White);

            DrawHelper.StartDrawWorldSpace(batch, BlendState.AlphaBlend);

            texture = renderTarget;
        }

        public virtual void Draw(BaseWorld world, SpriteBatch batch)
        {
            try
            {
                Vector2 pos = new Vector2(BaseMain.camera.Position.X % (BaseMain.WIDTH * 2), BaseMain.camera.Position.Y % (BaseMain.HEIGHT * 2));       //get the 'wrapped' camera position
                Rectangle source = new Rectangle(((-BaseMain.camera.Position * scrollRate)).ToPoint(), new Point(BaseMain.WIDTH, BaseMain.HEIGHT)); //source rectangle position divided in half because we'd be translating two times otherwise.
                //batch.Draw(renderTarget, Vector2.Zero, source, Color.White, 0, -pos, 1, SpriteEffects.None, 0);
                batch.Draw(renderTarget, BaseMain.camera.Position, source, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                /*Vector2 pos = new Vector2(world.entities[0].position.X % (BaseMain.WIDTH), world.entities[0].position.Y % (BaseMain.HEIGHT));       //get the 'wrapped' camera position
                Rectangle source = new Rectangle(((-world.entities[0].position * scrollRate)).ToPoint(), new Point(BaseMain.WIDTH, BaseMain.HEIGHT)); //source rectangle position divided in half because we'd be translating two times otherwise.
                batch.Draw(renderTarget, Vector2.Zero, source, Color.White, 0, -world.entities[0].position + (new Vector2(BaseMain.WIDTH, BaseMain.HEIGHT) / 2), 1, SpriteEffects.None, 0);*/
                //batch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            catch
            {
                DrawRenderTarget(batch);
            }
        }
    }
}
