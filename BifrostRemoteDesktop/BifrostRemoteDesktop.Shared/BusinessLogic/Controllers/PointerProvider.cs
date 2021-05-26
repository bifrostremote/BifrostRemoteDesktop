using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Controls
{

    public interface IPointerProvider
    {
        IPointerController GetPointerController();
    }

    public interface IPlatformPointerProvider
    {
        IPointerController GetPlatformPointerController();
    }

    public partial class PointerProvider : IPointerProvider
    {
        public IPointerController GetPointerController()
        {
            this.GetPlatformPointerController();
            throw new NotImplementedException("This feature is not implemented on the current platform.");
        }
    }
}
