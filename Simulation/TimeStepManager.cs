using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Simulation
{
    /// <summary>
    /// https://medium.com/@tglaiel/how-to-make-your-game-run-at-60fps-24c61210fe75
    /// </summary>
    public class TimeStepManager
    {
        public delegate void UpdateAction(float dt);
        public event UpdateAction OnUpdate;

        protected float timestep;
        protected float timestepClampUpper;
        protected float timestepClampLower;
        protected float timescale = 1.0f;

        protected float accumulator;

        protected bool active;
        protected bool resync;

        public TimeStepManager(float tickRate, float timescale, float tickRateUpperClamp, float tickRateLowerClamp)
        {
            accumulator = 0;
            this.timestep = 1.0f / tickRate;
            timestepClampLower = 1.0f / tickRateLowerClamp;
            timestepClampUpper = 1.0f / tickRateUpperClamp;
            this.timescale = timescale;
            resync = false;
            active = false;
        }

        /// <summary>
        /// Should be called after swapping scenes or changing levels.
        /// </summary>
        public void Resync()
        {
            resync = true;
        }

        public void Clear()
        {
            accumulator = 0;
        }

        public void Activate()
        {
            active = true;
        }

        public void Deactivate()
        {
            active = false;
        }

        public void Update(float delta)
        {
            if (!active)
            {
                return;
            }
            if (resync)
            {
                accumulator = 0;
                delta = timestep;
                resync = false;
            }

            if (Mathf.Abs(delta - timestepClampUpper) < .0002)
            {
                delta = timestepClampUpper;
            }
            if (Mathf.Abs(delta - timestep) < .0002)
            {
                delta = timestep;
            }
            if (Mathf.Abs(delta - timestepClampLower) < .0002)
            {
                delta = timestepClampLower;
            }

            accumulator += delta * timescale;

            while (accumulator >= timestep)
            {
                ManualUpdate(timestep);
                accumulator -= timestep;
            }
        }

        public void ManualUpdate(float dt)
        {
            OnUpdate?.Invoke(dt);
        }
    }
}
