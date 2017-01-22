using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LeyStoneEngine;
using LeyStoneEngine.Utility;
using LeyStoneEngine.Interface;
using LeyStoneEngine.Collision;

namespace LeyStoneEngine.Entities
{
    public abstract class Entity
    {
        public Vector2 velocity;

        public float speedMult = 1;

        public Vector2 position;

        public Polygon hitpoly;
        public bool solid = true;

        protected bool collided = false, resetYVel = false, resetXVel = false;

        public readonly int entityType;     //Type 0 is reserved for the player entity. Type 1 is reserved for visual effects.
        public readonly int entitySubType;

        public bool dead;
        public Action<BaseWorld, Entity> dieFunc;

        public Timer alive;

        public Entity(Vector2 position, Vector2 hitboxSize, dynamic entityType, dynamic entitySubType)
        {
            this.position = position;

            hitpoly = new PolyRectangle(position, position + hitboxSize);

            this.entityType = (int)entityType;
            this.entitySubType = (int)entitySubType;

            alive = Timer.CreateTimer(0, BaseWorld.timeScale, false, false, true);
        }

        public Entity(Vector2 position)
        {
            this.position = position;
        }

        public virtual void Move()
        {
            Vector2 vel = new Vector2((velocity.X * speedMult) * BaseWorld.timeScale.scale, (velocity.Y * speedMult) * BaseWorld.timeScale.scale);
            hitpoly.Move(vel);
            //hitbox.MoveTo(position);
            position = hitpoly.center;
        }

        /// <summary>
        /// Update method for entities.
        /// NOTE: do movement of entity <i>before</i> base.Update.
        /// </summary>
        public virtual void Update(BaseWorld world)
        {
            if (alive.time == 0)
            {
                if (solid && hitpoly == null)
                    throw new EntityFormatException("Entity is solid but does not have a defined hitbox!");
            }

            //alive++;

            collided = false;
            resetYVel = false;
            resetXVel = false;

            if (solid)
            {
                PostResolve(world);
            }
        }

        public virtual void PostResolve(BaseWorld world)
        {
            List<Vector2> resolvedVectors = hitpoly.CheckResolveCollision(world);
            if (resolvedVectors.Count > 0)
            {
                collided = true;

                resolvedVectors.ForEach(vector =>
                {
                    float angle = vector.GetVectorAngle();
                    if ((angle >= 45 && angle <= 135) || (angle >= 225 && angle <= 315))
                    {
                        velocity.Y = 0;
                        resetYVel = true;
                    }
                    else if ((angle >= 0 && angle < 45) || (angle > 315 && angle <= 360) || (angle > 135 || angle < 225))
                    {
                        velocity.X = 0;
                        resetXVel = true;
                    }
                });
            }
        }

        public virtual void Die(BaseWorld world)
        {
            dead = true;
            dieFunc?.Invoke(world, this);
        }

        public abstract void Draw(SpriteBatch batch);
    }
}
