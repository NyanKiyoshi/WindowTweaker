using System;
using System.Drawing;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace WindowTweaker {
    public class WindowGrabber : Form {
        private Point? _selectedLocation = null;
        public Point? SelectedLocation => _selectedLocation;

        public WindowGrabber() {
            InitializeComponent();
            User32.SetWindowTopMost(this.Handle);

            var targetLocation = new Point();
            User32.GetCursorPos(ref targetLocation);
            this.MoveWindowToPoint(targetLocation);

            var globalHook = Hook.GlobalEvents();
            globalHook.MouseMove += this.MouseHook_OnMouseMove;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.SuspendLayout();
            this.ClientSize = new Size(50, 50);

            this.AutoSizeMode = AutoSizeMode.GrowOnly;

            // Make the form fully transparent
            this.TransparencyKey = Color.Turquoise;
            this.BackColor = Color.Turquoise;

            // Hide the window borders and decorations
            this.FormBorderStyle = FormBorderStyle.None;

            this.Name = "Select a target window";
            this.ResumeLayout(false);
        }

        #region Drawing
        protected void MoveWindowToPoint(Point basePoint) {
            this.Location = new Point(
                basePoint.X - this.ClientRectangle.Width / 2,
                basePoint.Y - this.ClientRectangle.Height / 2);
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            Pen redPen = new Pen(Color.Red, 1);
            var centerY = this.ClientRectangle.Height / 2;
            var centerX = this.ClientRectangle.Width / 2;

            e.Graphics.DrawLine(redPen, new Point(0, centerY), new Point(Width, centerY));
            e.Graphics.DrawLine(redPen, new Point(centerX, 0), new Point(centerX, Height));
        }
        #endregion

        #region MouseEvents
        protected void MouseHook_OnMouseMove(object sender, MouseEventArgs e) {
            this.MoveWindowToPoint(e.Location);
        }

        protected override void OnClick(EventArgs e) {
            base.OnClick(e);

            // Retrieve the event cursor's position
            var eventLocation = new Point();
            User32.GetCursorPos(ref eventLocation);
            this._selectedLocation = eventLocation;

            // Close the form and let the parent proceed with the retrieved data
            this.Close();
        }
        #endregion
    }
}
