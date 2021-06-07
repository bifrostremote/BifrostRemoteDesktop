using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BifrostRemoteDesktop.BusinessLogic.Controllers
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

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint GetLastError();

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

        public void SetCursorPosition(double x, double y)
        {
            var a = SetCursorPos(Convert.ToInt32(x), Convert.ToInt32(y));
            var b = GetLastError();
        }
    }
}
