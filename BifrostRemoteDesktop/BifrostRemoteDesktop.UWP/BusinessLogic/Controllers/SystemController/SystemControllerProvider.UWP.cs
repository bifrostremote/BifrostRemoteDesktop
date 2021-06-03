using BifrostRemoteDesktop.BusinessLogic.Controllers.SystemController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BifrostRemoteDesktop.BusinessLogic.Controllers
{
    public partial class SystemControllerProvider : ISystemControllerProvider
    {
        public ISystemController GetPlatformSystemController()
        {
            return new UWPSystemController();
        }
    }
}
