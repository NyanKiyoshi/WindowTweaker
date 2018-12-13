using System;
using System.Windows.Forms;
using System.Drawing;

namespace WindowTweaker {
    class Program {
        const byte DEFAULT_OPACITY = 150;
        static byte WantedOpacity = byte.MaxValue;

        static void Initialize() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        static void PromptOpacityValue() {
            var PromptForm = new InputBox(
                "What opacity? (byte)", DEFAULT_OPACITY.ToString());

            if (PromptForm.ShowDialog() != DialogResult.OK) {
                Environment.Exit(0);
            }

            if (!byte.TryParse(PromptForm.InputText, out WantedOpacity)) {
                MessageBox.Show(
                    "Invalid byte value (must be 0-255).", "Invalid input",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        static void SetWindowOpacity(IntPtr targetHWnd) {
            var WindowLongPtr = (uint) User32.GetWindowLongPtr(
                targetHWnd, User32.GWL_EXSTYLE) ^ User32.WS_EX_LAYERED;

            User32.SetWindowLong(
                targetHWnd, User32.GWL_EXSTYLE, (IntPtr)WindowLongPtr);

            KernelError.ShowAndExitIfError(
                User32.SetLayeredWindowAttributes(targetHWnd, 0, WantedOpacity, User32.LWA_ALPHA));
        }

        static void Main(string[] args) {
            Initialize();
            PromptOpacityValue();

            // Create the main form
            var windowGrabber = new WindowGrabber();
            windowGrabber.ShowDialog();

            // If a window was grabbed, change its opacity;
            // Otherwise, show an error.
            if (windowGrabber.SelectedLocation != null) {
                var targetHWnd = User32.WindowFromPoint(
                    (Point) windowGrabber.SelectedLocation);
                var windowTitle = User32.GetWindowTextString(targetHWnd);

                SetWindowOpacity(targetHWnd);
                MessageBox.Show(string.Format(
                    "Set opacity of {0} to {1}", windowTitle, WantedOpacity));
            }
            else {
                MessageBox.Show(
                    "Nothing was selected.", "Operation aborted",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
