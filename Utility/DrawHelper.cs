using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LeyStoneEngine.Utility
{
    public static class DrawHelper
    {
        public static SamplerState primitiveSampler = new SamplerState { AddressU = TextureAddressMode.Wrap, AddressV = TextureAddressMode.Wrap, AddressW = TextureAddressMode.Wrap };
        #region DrawContexts

        public static void ChangeDrawContext(SpriteBatch batch, SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterState, Effect effect, Matrix transformMatrix)
        {
            batch.End();
            batch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterState, effect, transformMatrix);
        }

        public static void StartDrawCameraSpace(SpriteBatch batch, BlendState bState, bool endoverride = false)
        {
            if (!endoverride)
                batch.End();
            batch.Begin(SpriteSortMode.Deferred, bState, primitiveSampler, null, null, null);
        }

        public static void StartDrawWorldSpace(SpriteBatch batch, BlendState bState, bool endoverride = false)
        {
            if (!endoverride)
                batch.End();
            batch.Begin(SpriteSortMode.Deferred, bState, primitiveSampler, null, null, null, BaseMain.camera.GetViewMatrix());
        }

        #endregion
    }
}
