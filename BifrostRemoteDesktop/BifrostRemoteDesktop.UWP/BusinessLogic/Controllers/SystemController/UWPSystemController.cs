using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BifrostRemoteDesktop.BusinessLogic.Controllers.SystemController
{
    class UWPSystemController : ISystemController
    {
        private UWPPointerController _pointer;

        public UWPSystemController()
        {
            _pointer = new UWPPointerController();
        }

        public bool SetPointerPosition(double x, double y)
        {
            _pointer.SetCursorPosition(x, y);
            return true;
        }

        public void SetPointerState()
        {
            throw new NotImplementedException();
        }

    }
}
