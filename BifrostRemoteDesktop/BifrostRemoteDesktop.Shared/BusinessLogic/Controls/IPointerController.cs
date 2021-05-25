using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BifrostRemoteDesktop.BusinessLogic
{
    public interface IPointerController
    {
        bool CanSetCursorPosition { get; }
        void SetCursorPosition(int x, int y);

        bool CanGetCursorPosition { get; }
        LPPoint GetCursorPosition();
    }
}
