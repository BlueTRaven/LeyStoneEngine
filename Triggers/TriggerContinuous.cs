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

namespace LeyStoneEngine.Triggers
{
    public class TriggerContinuous : Trigger
    {
        public TriggerContinuous(Vector2 position, Vector2 size, int triggeredByType, Action<BaseWorld, Trigger> triggerAction) : base(position, size, triggeredByType, triggerAction, 1)
        {
        }

        public override void CheckTrigger(BaseWorld world)
        {
            foreach (Entity e in world.entities)
            {
                if (e.entityType == triggeredByType)
                {
                    if (bounds.Contains(e.position))
                    {
                        triggerAction?.Invoke(world, this);
                    }
                }
            }
        }
    }
}
