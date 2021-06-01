using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Models.Commands
{
    public class PointerUpdateStateCommand : RemoteControlCommand<PointerUpdateStateCommandArgs>
    {
        private readonly PointerUpdateStateCommandArgs _args;

        public PointerUpdateStateCommand(
            ISystemController systemController, PointerUpdateStateCommandArgs args) : base(systemController)
        {
            this._args = args;
        }

        public override void Execute()
        {
            _systemController.SetPointerState();
        }
    }
}
