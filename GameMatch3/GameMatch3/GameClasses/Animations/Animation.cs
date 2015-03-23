namespace GameMatch3.Animations
{
    /// <summary>
    /// Base animation class.
    /// </summary>
    abstract class Animation
    {
        /// <summary>
        /// Animation speed in seconds.
        /// </summary>
        protected readonly double Speed;

        /// <summary>
        /// Animation duration in seconds.
        /// </summary>
        protected double Time;

        /// <summary>
        /// Returns the percentage of the animation complete.
        /// </summary>
        public double CompletedPersent { get; protected set; }

        /// <summary>
        /// Base animation class constructor.
        /// </summary>
        /// <param name="speed">Animation speed in seconds</param>
        protected Animation(double speed)
        {
            Speed = speed;
            Time = 0;
        }

        /// <summary>
        /// Changes animation state.
        /// </summary>
        /// <param name="time">Time in seconds elapsed since the last render call</param>
        public virtual void DeltaTime(double time)
        {
            Time += time;

            CompletedPersent = Time / Speed;
            if (CompletedPersent >= 1)
            {
                CompletedPersent = 1;
            }
        }
    }
}