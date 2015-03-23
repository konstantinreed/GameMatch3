using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameMatch3.Animations;

namespace GameMatch3
{
    /// <summary>
    /// Match3 game class. It provides methods for the generation, manipulation, and rendering of the game field.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Minimal sequence elements to destroy.
        /// </summary>
        private const int CellMinMatchCount = 3;

        /// <summary>
        /// Count of elements types.
        /// </summary>
        private const int CellTypesCount = 6;

        //Constants for the animation of the game board.
        private const int CellDestroyPoint = 10;
        private const double CellDestroySpeed = 0.3;
        private const double CellFallingSpeed = 0.4;
        private const double CellSwapSpeed = 0.3;

        //Constants for the design of the game board.
        private readonly Brush _backgroundColor = Brushes.LightGray;
        private readonly Pen _borderPen = new Pen(Color.DarkGray);
        private readonly Pen _borderSelectedPen = new Pen(Color.Black);
        private readonly Color[] _cellTypesColors = { Color.Blue, Color.DarkGreen, Color.Red, Color.Olive, Color.Purple, Color.DarkSlateGray };
        private const int CellCirclePadding = 5;
        
        
        /// <summary>
        /// Game canvas.
        /// </summary>
        private readonly Graphics _graphics;

        /// <summary>
        /// Size of square game field.
        /// </summary>
        private readonly int _fieldSize;

        /// <summary>
        /// Pixer size of square game field's cell.
        /// </summary>
        private readonly int _cellSize;

        /// <summary>
        /// Array of game field's cells.
        /// </summary>
        private readonly int[,] _field;


        /// <summary>
        /// True, if the user can't manipulate game field.
        /// </summary>
        private bool _isLocked = true;

        /// <summary>
        /// True, if a game field's cell was selected.
        /// </summary>
        private bool _isCellSelected;

        /// <summary>
        /// Сoordinates of the selected game field's cell.
        /// </summary>
        private Point _cellSelected;

        /// <summary>
        /// Current animation of game field.
        /// </summary>
        private Animation _animation;

        /// <summary>
        /// True, if a game ends.
        /// </summary>
        private bool _isStopping;

        /// <summary>
        /// True, if a game was finished.
        /// </summary>
        public bool IsStopped { get; private set; }

        private readonly Random _random = new Random();
        

        /// <summary>
        /// Match3 game class constructor.
        /// </summary>
        /// <param name="graphics">Game canvas</param>
        /// <param name="canvasSize">Pixer size of square game canvas</param>
        /// <param name="fieldSize">Size of square game field</param>
        public Game(Graphics graphics, int canvasSize, int fieldSize)
        {
            _graphics = graphics;
            _fieldSize = fieldSize;

            _field = new int[_fieldSize, _fieldSize];
            _cellSize = canvasSize / _fieldSize;

            GenerateField();
            RenderField();

            CheckMathes();
        }

        public int GamePoints { get; private set; }

        /// <summary>
        /// Handles mouse click on the game canvas.
        /// </summary>
        /// <param name="x">X mouse click coordinate</param>
        /// <param name="y">Y mouse click coordinate</param>
        public void FieldMouseClick(int x, int y)
        {
            if (_isLocked || _isStopping) return;

            var cellX = x / _cellSize;
            var cellY = y / _cellSize;

            if (cellX < 0 || cellX >= _fieldSize || cellY < 0 || cellY >= _fieldSize) return;

            if (!_isCellSelected)
            {
                _isCellSelected = true;
                _cellSelected = new Point(cellX, cellY);
                RenderCell(cellX, cellY);
            }
            else
            {
                _isCellSelected = false;
                RenderCell(_cellSelected.X, _cellSelected.Y);

                //if this cells are neighbors 
                if (Math.Abs(_cellSelected.X - cellX) + Math.Abs(_cellSelected.Y - cellY) == 1)
                {
                    _isLocked = true;
                    _animation = new SwappingAnimation(CellSwapSpeed, _cellSelected.X, _cellSelected.Y, _field[_cellSelected.X, _cellSelected.Y], cellX, cellY, _field[cellX, cellY]);
                }
            }
        }

        /// <summary>
        /// Stop the game.
        /// </summary>
        public void Stop()
        {
            _isStopping = true;
        }


        /// <summary>
        /// Generate pseudo random element type.
        /// </summary>
        private int RandomElement { get { return _random.Next(1, CellTypesCount + 1); }}

        private void GenerateField()
        {
            for (var i = 0; i < _fieldSize; ++i)
            {
                for (var j = 0; j < _fieldSize; ++j)
                {
                    _field[i, j] = RandomElement;
                }
            }
        }

        private void RenderField()
        {
            for (var i = 0; i < _fieldSize; ++i)
            {
                for (var j = 0; j < _fieldSize; ++j)
                {
                    RenderCell(i, j);
                }
            }
        }

        private void RenderCell(int x, int y)
        {
            var rectangleCell = CellToRectangle(x, y);
            var rectangleCircle = CellToCircleRectangle(x, y);

            _graphics.FillRectangle(_backgroundColor, rectangleCell);

            if (_field[x, y] == 0) return;

            _graphics.FillEllipse(new SolidBrush(_cellTypesColors[_field[x, y] - 1]), rectangleCircle);

            if (!_isCellSelected || _cellSelected.X != x || _cellSelected.Y != y)
            {
                _graphics.DrawRectangle(_borderPen, rectangleCell);
            }
            else
            {
                _graphics.DrawRectangle(_borderSelectedPen, rectangleCell);
            }
        }

        private Rectangle CellToRectangle(int x, int y)
        {
            return new Rectangle(x * _cellSize, y * _cellSize, _cellSize, _cellSize);
        }

        private Rectangle CellToCircleRectangle(int x, int y)
        {
            return new Rectangle(x * _cellSize + CellCirclePadding, y * _cellSize + CellCirclePadding, _cellSize - CellCirclePadding * 2, _cellSize - CellCirclePadding * 2);
        }

        /// <summary>
        /// Сhecks the game field to the existence of sequences of cells to be destroyed.
        /// </summary>
        /// <returns>Returns true, if the sequences of cells were found.</returns>
        public bool CheckMathes()
        {
            var cellToDestroyList = new List<Point>();

            for (var i = 0; i < _fieldSize; ++i)
            {
                var matchX = 0;
                var matchY = 0;
                var matchCountX = 0;
                var matchCountY = 0;

                for (var j = 0; j < _fieldSize; ++j)
                {
                    //Check horizontal sequences of cells. i => y, j => x
                    if (matchX == 0)
                    {
                        matchX = _field[j, i];
                        matchCountX++;
                    }
                    else
                    {
                        if (matchX == _field[j, i])
                        {
                            matchCountX++;

                            if (j == _fieldSize - 1 && matchCountX >= CellMinMatchCount)
                            {
                                for (var k = j - matchCountX + 1; k <= j; ++k)
                                {
                                    cellToDestroyList.Add(new Point(k, i));
                                }
                            }
                        }
                        else
                        {
                            if (matchCountX >= CellMinMatchCount)
                            {
                                for (var k = j - matchCountX; k < j; ++k)
                                {
                                    cellToDestroyList.Add(new Point(k, i));
                                }
                            }

                            matchX = _field[j, i];
                            matchCountX = 1;
                        }
                    }

                    //Check vertical sequences of cells. i => x, j => y
                    if (matchY == 0)
                    {
                        matchY = _field[i, j];
                        matchCountY++;
                    }
                    else
                    {
                        if (matchY == _field[i, j])
                        {
                            matchCountY++;

                            if (j == _fieldSize - 1 && matchCountY >= CellMinMatchCount)
                            {
                                for (var k = j - matchCountY + 1; k <= j; ++k)
                                {
                                    cellToDestroyList.Add(new Point(i, k));
                                }
                            }
                        }
                        else
                        {
                            if (matchCountY >= CellMinMatchCount)
                            {
                                for (var k = j - matchCountY; k < j; ++k)
                                {
                                    cellToDestroyList.Add(new Point(i, k));
                                }
                            }

                            matchY = _field[i, j];
                            matchCountY = 1;
                        }
                    }
                }
            }

            if (cellToDestroyList.Count == 0)
            {
                _isLocked = false;
                return false;
            }

            _isLocked = true;

            //Remove identical game field's cells
            cellToDestroyList = cellToDestroyList.Distinct().ToList();

            GamePoints += cellToDestroyList.Count * CellDestroyPoint;

            _animation = new DestroyingAnimation(CellDestroySpeed, cellToDestroyList);

            return true;
        }

        /// <summary>
        /// Render destroy animation.
        /// </summary>
        private void RenderDestroyAnimation()
        {
            var destroyAnimation = _animation as DestroyingAnimation;
            if (destroyAnimation == null) return;

            foreach (var cell in destroyAnimation.Cells)
            {
                var rectangleCell = CellToRectangle(cell.X, cell.Y);
                var rectangleCircle = CellToCircleRectangle(cell.X, cell.Y);

                //Drawing background, border and element
                _graphics.FillRectangle(_backgroundColor, rectangleCell);
                _graphics.DrawRectangle(_borderPen, rectangleCell);
                _graphics.FillEllipse(new SolidBrush(Color.FromArgb(destroyAnimation.Alpha, _cellTypesColors[_field[cell.X, cell.Y] - 1])), rectangleCircle);
            }

            if (destroyAnimation.CompletedPersent < 1) return;

            //If animation is complete

            //Searching max y coordinate of each column contains deleted items
            var maxEmptyYOfLines = Enumerable.Repeat(-1, _fieldSize).ToArray();
            foreach (var cell in destroyAnimation.Cells)
            {
                _field[cell.X, cell.Y] = 0;
                if (maxEmptyYOfLines[cell.X] < cell.Y)
                {
                    maxEmptyYOfLines[cell.X] = cell.Y;
                }
            }

            //Prepararing falling animation and generate new elements
            var fallingAnimation = new FallingAnimation(CellFallingSpeed);
            for (var i = 0; i < _fieldSize; ++i)
            {
                if (maxEmptyYOfLines[i] == -1) continue;

                var fallingColumn = new FallingAnimation.Column(i, maxEmptyYOfLines[i]);

                for (var j = fallingColumn.MaxY; j >= 0; --j)
                {
                    if (_field[i, j] != 0)
                    {
                        fallingColumn.Cells.Add(new FallingAnimation.Column.Cell(j, _field[i, j]));
                    }
                }

                var newCellsCount = fallingColumn.MaxY - fallingColumn.Cells.Count + 1;
                for (var j = 1; j <= newCellsCount; ++j)
                {
                    fallingColumn.Cells.Add(new FallingAnimation.Column.Cell(j * -1, RandomElement));
                }

                fallingAnimation.Columns.Add(fallingColumn);
            }

            _animation = fallingAnimation;
        }

        /// <summary>
        /// Render fall animation.
        /// </summary>
        private void RenderFallingAnimation()
        {
            var fallingAnimation = _animation as FallingAnimation;
            if (fallingAnimation == null) return;

            foreach (var column in fallingAnimation.Columns)
            {
                var rectangleBack = new Rectangle(column.X * _cellSize, 0, _cellSize, (column.MaxY + 1) * _cellSize);

                //Drawing background
                _graphics.FillRectangle(_backgroundColor, rectangleBack);

                //Drawing borders
                for (var j = 0; j <= column.MaxY; ++j)
                {
                    var rectangleCell = CellToRectangle(column.X, j);
                    _graphics.DrawRectangle(_borderPen, rectangleCell);
                }

                //Drawing elements
                var cellList = column.Cells;
                for (var j = 0; j < cellList.Count; ++j)
                {
                    var rectangleCircle = CellToCircleRectangle(column.X, cellList[j].Y);
                    var offsetY = Convert.ToInt32((((column.MaxY - j) - cellList[j].Y) * _cellSize * fallingAnimation.CompletedPersent));
                    rectangleCircle.Offset(0, offsetY);

                    if (rectangleCircle.Y > -_cellSize)
                    {
                        _graphics.FillEllipse(new SolidBrush(_cellTypesColors[cellList[j].Element - 1]), rectangleCircle);
                    }
                }
            }

            if (fallingAnimation.CompletedPersent < 1) return;

            //If animation is complete

            //Refresh game field
            foreach (var column in fallingAnimation.Columns)
            {
                for (var j = 0; j < column.Cells.Count; ++j)
                {
                    _field[column.X, column.MaxY - j] = column.Cells[j].Element;
                }
            }
            _animation = null;

            CheckMathes();
        }

        /// <summary>
        /// Render swap animation.
        /// </summary>
        private void RenderSwappingAnimation()
        {
            var swapAnimation = _animation as SwappingAnimation;
            if (swapAnimation == null) return;

            var sourceRectangleBack = CellToRectangle(swapAnimation.SourceCell.X, swapAnimation.SourceCell.Y);
            var destinationRectangleBack = CellToRectangle(swapAnimation.DestinationCell.X, swapAnimation.DestinationCell.Y);

            //Drawing backgrounds
            _graphics.FillRectangle(_backgroundColor, sourceRectangleBack);
            _graphics.FillRectangle(_backgroundColor, destinationRectangleBack);

            //Drawing borders
            _graphics.DrawRectangle(_borderPen, sourceRectangleBack);
            _graphics.DrawRectangle(_borderPen, destinationRectangleBack);

            var sourceRectangleCircle = CellToCircleRectangle(swapAnimation.SourceCell.X, swapAnimation.SourceCell.Y);
            var destinationRectangleCircle = CellToCircleRectangle(swapAnimation.DestinationCell.X, swapAnimation.DestinationCell.Y);

            var offsetX = Convert.ToInt32(((swapAnimation.DestinationCell.X - swapAnimation.SourceCell.X) * _cellSize * swapAnimation.CompletedPersent));
            var offsetY = Convert.ToInt32(((swapAnimation.DestinationCell.Y - swapAnimation.SourceCell.Y) * _cellSize * swapAnimation.CompletedPersent));

            sourceRectangleCircle.Offset(offsetX, offsetY);
            destinationRectangleCircle.Offset(-offsetX, -offsetY);

            //Drawing elements
            _graphics.FillEllipse(new SolidBrush(_cellTypesColors[swapAnimation.SourceCell.Element - 1]), sourceRectangleCircle);
            _graphics.FillEllipse(new SolidBrush(_cellTypesColors[swapAnimation.DestinationCell.Element - 1]), destinationRectangleCircle);

            if (swapAnimation.CompletedPersent < 1) return;

            //If animation is complete

            //Swapping elements on game fields
            _field[swapAnimation.SourceCell.X, swapAnimation.SourceCell.Y] = swapAnimation.DestinationCell.Element;
            _field[swapAnimation.DestinationCell.X, swapAnimation.DestinationCell.Y] = swapAnimation.SourceCell.Element;

            if (!swapAnimation.IsReversed)
            {
                if (!CheckMathes())
                {
                    _isLocked = true;

                    //Reverce swapping elements on game fields
                    _field[swapAnimation.SourceCell.X, swapAnimation.SourceCell.Y] = swapAnimation.SourceCell.Element;
                    _field[swapAnimation.DestinationCell.X, swapAnimation.DestinationCell.Y] = swapAnimation.DestinationCell.Element;

                    swapAnimation.Reverse();
                }
            }
            else
            {
                _animation = null;
                _isLocked = false;
            }
        }

        /// <summary>
        /// Render game field.
        /// </summary>
        /// <param name="deltaTime">Time in seconds elapsed since the last render call</param>
        /// <param name="repaint">Set true if game canvas needs to be redraw fully</param>
        public void Render(double deltaTime, bool repaint = false)
        {
            if (repaint)
            {
                RenderField();
            }

            if (IsStopped) return;
            if (_isStopping && _animation == null)
            {
                IsStopped = true;
                return;
            }
            if (_animation == null) return;

            _animation.DeltaTime(deltaTime);

            if (_animation is DestroyingAnimation)
            {
                RenderDestroyAnimation();
            }
            else if (_animation is FallingAnimation)
            {
                RenderFallingAnimation();
            }
            else if (_animation is SwappingAnimation)
            {
                RenderSwappingAnimation();
            }
        }
    }
}
