using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameMatch3
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// Pixer size of square game canvas.
        /// </summary>
        private const int CanvasSize = 400;

        /// <summary>
        /// Size of square game field.
        /// </summary>
        private const int FieldSize = 8;

        /// <summary>
        /// Time for game in seconds.
        /// </summary>
        private const int PlayingTime = 60;

        /// <summary>
        /// String format for points label.
        /// </summary>
        private const string PointsStr = "Points: {0}";

        /// <summary>
        /// String format for time label.
        /// </summary>
        private const string TimeStr = "Time: {0}";


        private readonly Graphics _graphics;


        /// <summary>
        /// Match 3 game object.
        /// </summary>
        private Game _game;


        private bool _isGameRunning;
        private int _gamePoints;
        private int _gameTime;
        private DateTime _gameStartTime;
        private DateTime _lastGameRenderTime;
        

        public FormMain()
        {
            InitializeComponent();

            _graphics = _panelCanvas.CreateGraphics();

            ScreenMainMenu();
        }

        /// <summary>
        /// Sets controls for main menu.
        /// </summary>
        public void ScreenMainMenu()
        {
            _buttonPlay.Visible = true;
            _buttonGameOver.Visible = false;
            _labelTime.Visible = false;
            _labelPoints.Visible = false;
            _labelGameOver.Visible = false;

            _isGameRunning = false;
        }

        /// <summary>
        /// Sets controls for playing.
        /// </summary>
        public void ScreenGameStart()
        {
            _buttonPlay.Visible = false;
            _buttonGameOver.Visible = false;
            _labelTime.Visible = true;
            _labelPoints.Visible = true;
            _labelGameOver.Visible = false;

            _gamePoints = 0;
            _gameTime = 0;
            RefreshGameStats(_gamePoints, _gameTime);

            _game = new Game(_graphics, CanvasSize, FieldSize);
            _isGameRunning = true;
            _gameStartTime = DateTime.Now;
            _lastGameRenderTime = DateTime.Now;

            _timerCanvasRefresh.Start();
        }

        /// <summary>
        /// Sets controls for game over menu.
        /// </summary>
        public void ScreenGameOver()
        {
            _buttonPlay.Visible = false;
            _buttonGameOver.Visible = true;
            _labelTime.Visible = false;
            _labelPoints.Visible = true;
            _labelGameOver.Visible = true;

            _isGameRunning = false;

            _timerCanvasRefresh.Stop();
        }

        /// <summary>
        /// Refreshing point and time labels.
        /// </summary>
        private void RefreshGameStats(int points, int time)
        {
            if (_gamePoints != points)
            {
                _gamePoints = points;
                _labelPoints.Text = String.Format(PointsStr, _gamePoints);
            }

            if (_gameTime != time)
            {
                _gameTime = time;
                _labelTime.Text = String.Format(TimeStr, _gameTime);
            }
        }

        /// <summary>
        /// Paint game canvas.
        /// </summary>
        /// <param name="full">Set true if game canvas needs to be redraw fully</param>
        private void Repaint(bool full = false)
        {
            if (_game == null) return;

            var deltaTime = (DateTime.Now - _lastGameRenderTime).TotalMilliseconds / 1000;
            _lastGameRenderTime = DateTime.Now;
            _game.Render(deltaTime, full);

            if (!_isGameRunning) return;

            var playingTime = PlayingTime - Convert.ToInt32((DateTime.Now - _gameStartTime).TotalSeconds);
            if (playingTime <= 0)
            {
                playingTime = 0;
                _game.Stop();
            }

            RefreshGameStats(_game.GamePoints, playingTime);

            if (_game.IsStopped)
            {
                ScreenGameOver();
            }
        }

        private void ButtonPlayClick(object sender, MouseEventArgs e)
        {
            ScreenGameStart();
        }

        private void ButtonGameOverClick(object sender, MouseEventArgs e)
        {
            ScreenMainMenu();
        }

        private void CanvasMouseClick(object sender, MouseEventArgs e)
        {
            if (!_isGameRunning) return;

            _game.FieldMouseClick(e.X, e.Y);
        }

        private void TimerCanvasRefresh(object sender, EventArgs e)
        {
            Repaint();
        }

        private void PanelCanvasPaint(object sender, PaintEventArgs e)
        {
            _timerCanvasRefresh.Stop();
            Repaint(true);
            _timerCanvasRefresh.Start();
        }
    }
}
