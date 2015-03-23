using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameMatch3.Animations
{
    class DestroyingAnimation : Animation
    {
        /// <summary>
        /// Max transparent value.
        /// </summary>
        private const int AlphaMaxValue = 255;


        /// <summary>
        /// List of game field's cells to destroy.
        /// </summary>
        public List<Point> Cells { get; private set; }

        /// <summary>
        /// Current alpha value.
        /// </summary>
        public int Alpha { get; private set; }

        /// <summary>
        /// Destroy animation class constructor.
        /// </summary>
        /// <param name="cellDestroySpeed">Animation speed in seconds</param>
        /// <param name="cells">List of game field's cells to destroy</param>
        public DestroyingAnimation(double cellDestroySpeed, List<Point> cells)
            : base(cellDestroySpeed)
        {
            Cells = cells;
        }

        /// <summary>
        /// Changes animation state.
        /// </summary>
        /// <param name="time">Time in seconds elapsed since the last render call</param>
        public override void DeltaTime(double time)
        {
            base.DeltaTime(time);

            Alpha = Convert.ToInt32(AlphaMaxValue - AlphaMaxValue * CompletedPersent);
        }
    }
}