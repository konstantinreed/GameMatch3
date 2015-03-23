namespace GameMatch3.Animations
{
    class SwappingAnimation : Animation
    {
        public class Cell
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Element { get; set; }

            public Cell(int x, int y, int element)
            {
                X = x;
                Y = y;
                Element = element;
            }
        }

        /// <summary>
        /// Source swap game field's cell.
        /// </summary>
        public Cell SourceCell { get; private set; }

        /// <summary>
        /// Destination swap game field's cell.
        /// </summary>
        public Cell DestinationCell { get; private set; }

        /// <summary>
        /// Returns true, if swap was canceled.
        /// </summary>
        public bool IsReversed { get; private set; }

        /// <summary>
        /// Swapping animation class constructor.
        /// </summary>
        /// <param name="swapSpeed">Animation speed in seconds</param>
        /// <param name="srcX">Source game field's cell X coordinate</param>
        /// <param name="srcY">Source game field's cell Y coordinate</param>
        /// <param name="srcElement">Source game field's cell element</param>
        /// <param name="destX">Destination game field's cell X coordinate</param>
        /// <param name="destY">Destination game field's cell Y coordinate</param>
        /// <param name="destElement">Destination game field's cell element</param>
        public SwappingAnimation(double swapSpeed, int srcX, int srcY, int srcElement, int destX, int destY, int destElement)
            : base(swapSpeed)
        {
            SourceCell = new Cell(srcX, srcY, srcElement);
            DestinationCell = new Cell(destX, destY, destElement);
        }

        /// <summary>
        /// Cancels swap.
        /// </summary>
        public void Reverse()
        {
            var tmpCell = SourceCell;
            SourceCell = DestinationCell;
            DestinationCell = tmpCell;

            var tmpElement = SourceCell.Element;
            SourceCell.Element = DestinationCell.Element;
            DestinationCell.Element = tmpElement;

            IsReversed = true;

            DeltaTime(-Time);
        }
    }
}
