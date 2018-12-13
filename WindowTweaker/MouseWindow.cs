using System;
using System.Drawing;

namespace WindowTweaker {

    public class MouseWindow {

        public static IntPtr GetWindowFromCursor() {
            var point = new Point();
            User32.GetCursorPos(ref point);
            return User32.WindowFromPoint(point);
        }

    }

}
