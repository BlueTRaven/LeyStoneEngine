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
    public abstract class EntityLiving : Entity
    {
        public int health;

        public Timer invulnerableTimer;

        public EntityLiving(Vector2 position, Vector2 size, int health, dynamic entityType, dynamic entitySubtype) : base(position, size, (int)entityType, (int)entitySubtype)
        {
            this.health = health;

            this.invulnerableTimer = Timer.CreateTimer(0, BaseWorld.timeScale);
        }

        public virtual void TakeDamage(BaseWorld world, Entity dealer, int amount)
        {
            if (invulnerableTimer.done)
            {
                int finalAmount = CalculateDamage(amount);

                if (health - finalAmount <= 0)
                {
                    health = 0; //Yes, I do bother setting health to zero in the case Die is overwritten to not kill the entity.
                    Die(world);
                }
                else
                    health -= finalAmount;
            }
        }

        /// <summary>
        /// Calculates the amount of damage modified by other variables - eg. crit 2x, or defense .5x
        /// </summary>
        /// <param name="amount">The input amount of damage</param>
        public virtual int CalculateDamage(int amount)
        {
            return amount;
        }
    }
}
