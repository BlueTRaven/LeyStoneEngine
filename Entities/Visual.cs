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

namespace LeyStoneEngine.Entities
{
    public abstract class Visual : Entity
    {
        public int duration = -1, delay = -1;

        protected bool gravityAffected;
        protected float gravity;

        protected bool spins;
        protected float spinSpeed;
        protected float rotation;
        protected Vector2 offset;

        protected Timer fadeTimer;
        protected bool fades;
        protected int fadeStartTime;
        //protected int fadeEndTime;
        protected Color fadeToColor;
        protected bool diesEndFade = true;

        protected Timer wanderBetweenTimer;
        protected bool wanders, wandering;
        protected int wanderStartTime, wanderEndTime;//, wanderBetweenTime, wanderBetweenTimeMax;
        protected Vector2 wanderMaxRange;
        protected float wanderSpeed;

        protected Color baseColor, currentColor;

        public Visual(Vector2 position, int subType) : base(position, new Vector2(64, 64), 1, subType)
        {
            
        }

        public override void Update(BaseWorld world)
        {
            if (delay < 0)
            {
                base.Update(world);

                if (spins)
                    rotation += spinSpeed;

                if (fades)
                {
                    if (alive >= fadeStartTime)
                    {
                        fadeTimer.paused = false;
                        
                        if (fadeTimer.done)
                            Die(world);
                    }

                    if (!fadeTimer.paused)
                    {
                        currentColor = Color.Lerp(fadeToColor, baseColor, ((float)fadeTimer.time / (float)(fadeTimer.setTime)));
                    }
                }

                if (wanders)
                {
                    wandering = false;
                    if (alive >= wanderStartTime && (alive < wanderEndTime || wanderEndTime == -1))
                    {
                        wandering = true;
                    }

                    if (wandering)
                    {
                        if (wanderBetweenTimer.time <= 0)
                        {
                            velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(MathHelper.ToRadians((float)BaseMain.rand.NextDouble(0, 360))));
                            wanderBetweenTimer.Reset((int)wanderBetweenTimer.setTime + BaseMain.rand.Next((int)wanderMaxRange.X, (int)wanderMaxRange.Y));
                            //wanderBetweenTime = wanderBetweenTimeMax + BaseMain.rand.Next((int)wanderMaxRange.X, (int)wanderMaxRange.Y);
                        }
                        //else wanderBetweenTime--;
                    }
                }

                if (gravityAffected)
                    velocity.Y += gravity;

                if (duration != -1)
                {
                    duration--;

                    if (duration <= 0)
                        Die(world);
                }
            }
            else delay--;
        }

        public virtual Visual SetSolid(Polygon hitpoly)
        {
            this.solid = true;
            this.hitpoly = hitpoly;

            return this;
        }

        public virtual Visual SetGravityAffected(float gravity)
        {
            this.gravityAffected = true;
            this.gravity = gravity;
            return this;
        }

        public virtual Visual SetDelay(int delay)
        {
            this.delay = delay;
            return this;
        }

        public virtual Visual SetDies(int duration)
        {
            this.duration = duration;
            return this;
        }

        /// <summary>
        /// Sets the Visual to 'wander' or move around in random directions.
        /// </summary>
        /// <param name="startTime">When to start wandering.</param>
        /// <param name="endTime">When to end wandering. Set to -1 to wander indefinitely.</param>
        /// <param name="timeBetween">How long between eacha direction of wandering.</param>
        /// <returns></returns>
        public virtual Visual SetWander(float speed, int startTime, int endTime, int timeBetween, Vector2 maxRange)
        {
            wanders = true;

            this.speedMult = speed;

            this.wanderStartTime = startTime;
            this.wanderEndTime = endTime;
            //this.wanderTimer = Timer.CreateTimer(endTime - startTime, true);

            this.wanderBetweenTimer = Timer.CreateTimer(timeBetween, BaseWorld.timeScale, true);
            //this.wanderBetweenTime = timeBetween;
            //this.wanderBetweenTimeMax = timeBetween;
            this.wanderMaxRange = maxRange;

            return this;
        }

        public virtual Visual SetSpin(Vector2 offset, float initialRotation, float speed)
        {
            this.spins = true;

            this.offset = offset;
            this.rotation = initialRotation;
            this.spinSpeed = speed;
            return this;
        }

        /// <summary>
        /// Sets the visual to 'fade' - go from one color to another.
        /// Please note that this does not intrinsicly change the color of what is being drawn; it only changes the currentColor variable. 
        /// Therefore things like polygons need to have their colors manually set, and by extention, gradients and polygons with triangles with different colors will not work.
        /// </summary>
        /// <param name="fadeStartTime">When to start fading, in ticks.</param>
        /// <param name="fadeEndTime">When the fading is completed, in ticks. This is not the duration of the fade, rather the ticks at which it is done.</param>
        /// <param name="fadeToColor">The color to fade to.</param>
        /// <param name="diesEndFade">Does the visual die at the end of the fade?</param>
        public virtual Visual SetFade(int fadeStartTime, int fadeEndTime, Color fadeToColor, bool diesEndFade = true)
        {
            this.fades = true;

            this.fadeStartTime = fadeStartTime; //Fadestart is necessary so I can know when to unpause the fadeTimer.
            //this.fadeEndTime = fadeEndTime;
            this.fadeTimer = Timer.CreateTimer(fadeEndTime - fadeStartTime, BaseWorld.timeScale, true);
            this.fadeToColor = fadeToColor;

            this.diesEndFade = diesEndFade;

            return this;
        }
    }
}
