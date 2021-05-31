using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Models.Commands
{
    public abstract class RemoteControlCommand : ICommand
    {
        protected readonly ISystemController _systemController;

        public RemoteControlCommand(ISystemController systemController)
        {
            _systemController = systemController;
        }

        public abstract void Execute();
    }
}
