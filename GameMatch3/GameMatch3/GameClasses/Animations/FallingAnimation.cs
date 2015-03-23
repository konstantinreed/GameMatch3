using System.Collections.Generic;

namespace GameMatch3.Animations
{
    class FallingAnimation : Animation
    {
        public class Column
        {
            public class Cell
            {
                public int Y { get; set; }
                public int Element { get; set; }

                public Cell(int y, int element)
                {
                    Y = y;
                    Element = element;
                }
            }

            public int X { get; set; }
            public int MaxY { get; set; }

            /// <summary>
            /// List of falling game field's cells.
            /// </summary>
            public List<Cell> Cells { get; private set; }

            public Column(int x, int maxY)
            {
                X = x;
                MaxY = maxY;
                Cells = new List<Cell>();
            }
        }

        /// <summary>
        /// List of falling game field's columns.
        /// </summary>
        public List<Column> Columns { get; private set; }

        /// <summary>
        /// Falling animation class constructor.
        /// </summary>
        /// <param name="fallingSpeed">Animation speed in seconds</param>
        public FallingAnimation(double fallingSpeed)
            : base(fallingSpeed)
        {
            Columns = new List<Column>();
        }
    }
}