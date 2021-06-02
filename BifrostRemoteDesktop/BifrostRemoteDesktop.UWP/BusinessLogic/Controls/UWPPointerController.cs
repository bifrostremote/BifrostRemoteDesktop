using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BifrostRemoteDesktop.BusinessLogic
{
    public class UWPPointerController : IPointerController
    {
        public bool CanSetCursorPosition => true;

        public bool CanGetCursorPosition => true;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out LPPoint lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetCursorPos(int x, int y);

        public LPPoint GetCursorPosition()
        {
            LPPoint point;
            if (GetCursorPos(out point))
            {
                return point;
            }
            else
            {
                // TODO: Narrow Exception Type.
                throw new Exception("LPPoint was not available through user32.dll");
            }
        }

        public void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }
    }
}
