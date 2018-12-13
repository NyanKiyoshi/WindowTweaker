using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowTweaker {

    internal class KernelError {

        public const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        public const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        public const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint FormatMessage(
            uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId,
            [MarshalAs(UnmanagedType.LPTStr)] ref string lpBuffer,
            int nSize, IntPtr[] Arguments);

        public static string GetErrorMessage(uint errorCode) {
            var source = IntPtr.Zero;
            var dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER
                | FORMAT_MESSAGE_FROM_SYSTEM
                | FORMAT_MESSAGE_IGNORE_INSERTS;
            string msgBuffer = string.Empty;

            FormatMessage(dwFlags, source, errorCode, 0, ref msgBuffer, 512, null);
            return msgBuffer.ToString();
        }

        public static void ShowAndExitIfError(uint resultCode) {
            if (resultCode != 1) {
                var errorCode = GetLastError();
                var errorMsg = (errorCode != 0)
                    ? string.Format("(0x{0:X8}) {1}", errorCode, GetErrorMessage(errorCode))
                    : "Something happened.";
                MessageBox.Show(
                    errorMsg, "Operation failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

    }

}
