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
using LeyStoneEngine.Serialization;

namespace LeyStoneEngine.Triggers
{
    public abstract class Trigger
    {
        public readonly Rectangle bounds;   //Bounds cannot be changed dynamically.

        public Action<BaseWorld, Trigger> triggerAction;

        public readonly int triggerType, triggeredByType;

        public bool dead;

        public int triggerActionIndex;  //Note this is only used for serialization. It's not deserialized, ever; therefore it will almost always be 0.

        public Trigger(Vector2 position, Vector2 size, int triggeredByType, Action<BaseWorld, Trigger> triggerAction, dynamic triggerType)
        {
            this.bounds = new Rectangle(position.ToPoint(), size.ToPoint());
            this.triggeredByType = triggeredByType;

            this.triggerAction = triggerAction;

            this.triggerType = (int)triggerType;
        }

        public abstract void CheckTrigger(BaseWorld world);

        public virtual void DrawDebug(SpriteBatch batch)
        {
            DrawPrimitives.DrawRectangle(batch, bounds, new Color(Color.Blue, 63));
            DrawPrimitives.DrawHollowRectangle(batch, bounds, 1, Color.Blue);
        }

        /// <summary>
        /// Returns a deserialized Trigger.
        /// </summary>
        /// <param name="serializedTrigger">The Serialized Trigger to deserialize.</param>
        /// <param name="action">The manually deserialized trigger action. Assign an int to an action and pass that to here.</param>
        public static Trigger Deserialize(SerTrigger serializedTrigger, Action<BaseWorld, Trigger> action)
        {
            if (serializedTrigger.Type == 0)
                return new TriggerOnce(serializedTrigger.Position.ToVector2(), serializedTrigger.Size.ToVector2(), serializedTrigger.TriggeredByType, action);
            else return new TriggerContinuous(serializedTrigger.Position.ToVector2(), serializedTrigger.Size.ToVector2(), serializedTrigger.TriggeredByType, action);
        }
    }
}
