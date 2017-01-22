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

namespace LeyStoneEngine
{
    public class Timer
    {
        public readonly float setTime;
        public float time;

        private readonly bool isCounter;

        public bool done = false, paused = false, loops = false;

        private TimeScale timeScale;

        public static Timer CreateTimer(int time, TimeScale timeScale, bool startsPaused = false, bool loops = false, bool isCounter = false)
        {
            Timer t = new Timer(time, isCounter, timeScale);
            t.paused = startsPaused;
            t.loops = loops;
            BaseMain.timers.Add(t);
            return t;
        }

        #region Operator Overrides
        public static bool operator !(Timer t)
        {
            return !t.done || !t.paused;
        }
        //Implemented so you don't have to use timer.time, e. g. you can now use (timer < 0) or (timer > 35)
        public static bool operator <(Timer t, int valueCompare)
        {
            return (t.time < valueCompare);
        }

        public static bool operator >(Timer t, int valueCompare)
        {
            return t.time > valueCompare;
        }

        public static bool operator <=(Timer t, int valueCompare)
        {
            return (t.time <= valueCompare);
        }

        public static bool operator >=(Timer t, int valueCompare)
        {
            return t.time >= valueCompare;
        }
        #endregion

        private Timer(int time, bool isCounter, TimeScale timeScale)
        {
            this.setTime = (float)time;
            this.time = (float)time;
            this.isCounter = isCounter;

            this.timeScale = timeScale;
        }

        public void Count()
        {   //updates first
            if (!paused)
            {
                if (isCounter)
                    time += 1 * timeScale.scale;
                else
                {
                    time -= 1 * timeScale.scale;
                }
            }
        }

        public void CheckDone()
        {   //run at end of main update method.
            if (time <= 0 && !isCounter)
            {
                if (!loops)
                    done = true;
                else Reset();
            }
        }

        public void Reset(int toValue = -1, bool noUnpause = false)
        {
            if (toValue == -1)
                time = setTime;
            else time = toValue;

            if (done)
            {
                if (!BaseMain.timers.Contains(this))
                    BaseMain.timers.Add(this);
                done = false;
            }

            if (paused && !noUnpause)
                paused = false;
        }
    }
}
