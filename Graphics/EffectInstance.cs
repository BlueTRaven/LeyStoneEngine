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
    public class EffectInstance
    {
        public readonly EffectContainer effectContainer;

        public bool invert; //Invert its use; eg. post will now run pre

        public EffectInstance(EffectContainer effectContainer, Action<EffectInstance> parameterFunc = null, bool invert = false)
        {
            this.effectContainer = effectContainer;

            SetParameters(parameterFunc);

            this.invert = invert;
        }

        public void SetParameters(Action<EffectInstance> parameterFunc)
        {
            parameterFunc?.Invoke(this);
        }

        public void Apply()
        {
            for (int i = 0; i < effectContainer.effect.CurrentTechnique.Passes.Count; i++)
            {
                effectContainer.effect.CurrentTechnique.Passes[i].Apply();
            }
        }
    }
}
