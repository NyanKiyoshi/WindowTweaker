using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowTweaker {
    internal class InputBox : Form {
        private readonly Button submitButton = new Button() { Text = "OK" };
        private readonly Button cancelButton = new Button() { Text = "Abort" };
        private readonly TextBox textBox = new TextBox() { Dock = DockStyle.Fill };
        public string InputText => textBox.Text;

        public InputBox(string labelText, string defaultValue) {
            this.SuspendLayout();

            var buttonContainer = new FlowLayoutPanel();
            var layoutContainer = new TableLayoutPanel();

            this.textBox.Text = defaultValue;

            buttonContainer.FlowDirection = FlowDirection.RightToLeft;
            buttonContainer.Dock = DockStyle.Fill;
            buttonContainer.Controls.Add(submitButton);
            buttonContainer.Controls.Add(cancelButton);

            layoutContainer.ColumnCount = 1;
            layoutContainer.RowCount = 3;
            layoutContainer.Dock = DockStyle.Fill;

            layoutContainer.Controls.Add(new Label() {
                TextAlign = ContentAlignment.MiddleCenter,
                Text = labelText,
                Dock = DockStyle.Fill
            });
            layoutContainer.Controls.Add(textBox);
            layoutContainer.Controls.Add(buttonContainer);

            this.submitButton.Click += OnSubmit_Click;
            this.cancelButton.Click += OnCancel_Click;

            this.ClientSize = new Size(250, 80);

            this.Name = "Prompt";
            this.AcceptButton = submitButton;
            this.CancelButton = cancelButton;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Controls.Add(layoutContainer);

            this.ResumeLayout(false);
        }

        public void OnSubmit_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void OnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
