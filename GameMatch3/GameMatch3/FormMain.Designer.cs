namespace GameMatch3
{
    partial class FormMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._panelCanvas = new System.Windows.Forms.Panel();
            this._labelGameOver = new System.Windows.Forms.Label();
            this._buttonGameOver = new System.Windows.Forms.Button();
            this._buttonPlay = new System.Windows.Forms.Button();
            this._labelTime = new System.Windows.Forms.Label();
            this._labelPoints = new System.Windows.Forms.Label();
            this._timerCanvasRefresh = new System.Windows.Forms.Timer(this.components);
            this._panelCanvas.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panelCanvas
            // 
            this._panelCanvas.Controls.Add(this._buttonGameOver);
            this._panelCanvas.Controls.Add(this._buttonPlay);
            this._panelCanvas.Location = new System.Drawing.Point(12, 27);
            this._panelCanvas.Name = "_panelCanvas";
            this._panelCanvas.Size = new System.Drawing.Size(402, 402);
            this._panelCanvas.TabIndex = 0;
            this._panelCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelCanvasPaint);
            this._panelCanvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CanvasMouseClick);
            // 
            // _labelGameOver
            // 
            this._labelGameOver.AutoSize = true;
            this._labelGameOver.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._labelGameOver.Location = new System.Drawing.Point(158, 0);
            this._labelGameOver.Name = "_labelGameOver";
            this._labelGameOver.Size = new System.Drawing.Size(102, 24);
            this._labelGameOver.TabIndex = 9;
            this._labelGameOver.Text = "GameOver";
            // 
            // _buttonGameOver
            // 
            this._buttonGameOver.Location = new System.Drawing.Point(150, 280);
            this._buttonGameOver.Name = "_buttonGameOver";
            this._buttonGameOver.Size = new System.Drawing.Size(100, 35);
            this._buttonGameOver.TabIndex = 8;
            this._buttonGameOver.Text = "Ok";
            this._buttonGameOver.UseVisualStyleBackColor = true;
            this._buttonGameOver.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonGameOverClick);
            // 
            // _buttonPlay
            // 
            this._buttonPlay.Location = new System.Drawing.Point(150, 280);
            this._buttonPlay.Name = "_buttonPlay";
            this._buttonPlay.Size = new System.Drawing.Size(100, 35);
            this._buttonPlay.TabIndex = 5;
            this._buttonPlay.Text = "Play";
            this._buttonPlay.UseVisualStyleBackColor = true;
            this._buttonPlay.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonPlayClick);
            // 
            // _labelTime
            // 
            this._labelTime.AutoSize = true;
            this._labelTime.Dock = System.Windows.Forms.DockStyle.Left;
            this._labelTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._labelTime.Location = new System.Drawing.Point(0, 0);
            this._labelTime.Name = "_labelTime";
            this._labelTime.Size = new System.Drawing.Size(83, 24);
            this._labelTime.TabIndex = 7;
            this._labelTime.Text = "Time: 60";
            // 
            // _labelPoints
            // 
            this._labelPoints.AutoSize = true;
            this._labelPoints.Dock = System.Windows.Forms.DockStyle.Right;
            this._labelPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._labelPoints.Location = new System.Drawing.Point(341, 0);
            this._labelPoints.Name = "_labelPoints";
            this._labelPoints.Size = new System.Drawing.Size(81, 24);
            this._labelPoints.TabIndex = 6;
            this._labelPoints.Text = "Points: 0";
            // 
            // _timerCanvasRefresh
            // 
            this._timerCanvasRefresh.Interval = 33;
            this._timerCanvasRefresh.Tick += new System.EventHandler(this.TimerCanvasRefresh);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 437);
            this.Controls.Add(this._labelGameOver);
            this.Controls.Add(this._panelCanvas);
            this.Controls.Add(this._labelTime);
            this.Controls.Add(this._labelPoints);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "Match 3";
            this._panelCanvas.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _panelCanvas;
        private System.Windows.Forms.Label _labelGameOver;
        private System.Windows.Forms.Button _buttonGameOver;
        private System.Windows.Forms.Label _labelTime;
        private System.Windows.Forms.Label _labelPoints;
        private System.Windows.Forms.Button _buttonPlay;
        private System.Windows.Forms.Timer _timerCanvasRefresh;
    }
}

