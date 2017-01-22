using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using LeyStoneEngine.Graphics;
using LeyStoneEngine.Utility;

namespace LeyStoneEngine
{
    public abstract class BaseAssets
    {
        public GraphicsDevice device;

        public Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

        //public Dictionary<string, Effect> shaders = new Dictionary<string, Effect>();
        public Dictionary<string, EffectContainer> effectContainers = new Dictionary<string, EffectContainer>();

        public Effect defaultEffect;

        public Texture2D GetTexture(string name)
        {
            try
            {
                return textures[name];
            }
            catch (KeyNotFoundException e)
            {
                Logger.Log("Couldn't find and/or load texture file '" + name + "'!\n" + e.ToString(), true);
                return textures["whitePixel"];
            }
        }

        public SpriteFont GetFont(string name)
        {
            try
            {
                return fonts[name];
            }
            catch (KeyNotFoundException e)
            {
                Logger.Log("Couldn't find and/or load font file '" + name + "'!\n" + e.ToString(), true);
                return null;
            }
        }

        /*public Effect GetEffect(string name)
        {
            try
            {
                return shaders[name];
            }
            catch (KeyNotFoundException e)
            {
                Logger.Log("Couldn't find and/or load shader file '" + name + "'!\n" + e.ToString(), true);
                return null;
            }
        }*/

        public EffectContainer GetEffectContainer(string name)
        {
            try
            {
                return effectContainers[name];
            }
            catch (KeyNotFoundException e)
            {
                Logger.Log("Couldn't find and/or load container file '" + name + "'!\n" + e.ToString(), true);
                return null;
            }
        }

        public SoundEffect GetSoundEffect(string name)
        {
            try
            {
                return soundEffects[name];
            }
            catch (KeyNotFoundException e)
            {
                Logger.Log("Couldn't find and/or load sound file '" + name + "'!\n" + e.ToString(), true);
                return null;
            }
        }

        /// <summary>
        /// Populates the resource lists. Run once at startup in Main.LoadContent.
        /// </summary>
        public virtual void Load(GraphicsDevice device, ContentManager manager)
        {
            BasicEffect basicEffect = new BasicEffect(device);
            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
                (0, device.Viewport.Width,     // left, right
                device.Viewport.Height, 0,    // bottom, top
                0, 1);
            basicEffect.Texture = GetTexture("whitePixel");

            effectContainers.Add("basicEffect", new EffectContainer(basicEffect, EffectUse.Pre));

            defaultEffect = effectContainers["basicEffect"].effect;

            //shaders.Add("bloomExtract", manager.Load<Effect>("Effects/bloom"));
            //shaders.Add("bloomCombine", manager.Load<Effect>("Effects/combineBloom"));
        }

        protected void LoadWhitePixel(GraphicsDevice device, ContentManager manager)
        {
            Texture2D whitePixel = new Texture2D(device, 1, 1);
            whitePixel.SetData<Color>(new Color[] { Color.White });
            textures.Add("whitePixel", whitePixel);
        }
    }
}