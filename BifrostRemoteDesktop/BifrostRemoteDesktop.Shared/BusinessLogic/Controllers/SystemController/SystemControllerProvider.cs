using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Controllers
{
    public partial class SystemControllerProvider : ISystemControllerProvider
    {

        public ISystemController GetSystemController()
        {
            return this.GetPlatformSystemController();
        }
    }
}
