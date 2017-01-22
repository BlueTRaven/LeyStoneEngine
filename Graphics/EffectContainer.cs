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
    public enum EffectUse
    {
        Pre,
        Post
    }

    public class EffectContainer
    {
        public readonly Effect effect;
        public readonly EffectUse use;

        public EffectContainer(Effect effect, EffectUse use)
        {
            this.effect = effect;
            this.use = use;
        }
    }
}
