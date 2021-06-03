using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BifrostRemoteDesktop.BusinessLogic.Controllers.SystemController
{
    class UWPSystemController : ISystemController
    {
        private UWPPointerController _pointerProvider;

        public UWPSystemController()
        {
            _pointerProvider = new UWPPointerController();
        }

        public bool SetPointerPosition(double x, double y)
        {
            _pointerProvider.SetCursorPosition((int)x, (int)y);
            return true;
        }

        public void SetPointerState()
        {
            throw new NotImplementedException();
        }

    }
}
