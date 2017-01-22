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
    public class TextureContainer
    {
        public readonly Texture2D texture;

        public Vector2 scale;

        public TextureContainer(Texture2D texture, Vector2 scale)
        {
            this.texture = texture;
            this.scale = scale;
        }

        public TextureContainer(string textureName, Vector2 scale)
        {
            this.texture = BaseMain.assets.GetTexture(textureName);
            this.scale = scale;
        }

        public TextureContainer(Texture2D texture, float scale)
        {
            this.texture = texture;
            this.scale = new Vector2(scale);
        }

        public TextureContainer(string textureName, float scale)
        {
            this.texture = BaseMain.assets.GetTexture(textureName);
            this.scale = new Vector2(scale);
        }
    }
}
