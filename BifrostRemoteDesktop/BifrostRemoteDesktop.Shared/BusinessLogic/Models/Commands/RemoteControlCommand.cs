using System;

namespace BifrostRemoteDesktop.BusinessLogic.Models.Commands
{
    public abstract class RemoteControlCommand<T> : ICommand where T : RemoteControlCommandArgs
    {
        protected readonly ISystemController _systemController;

        public RemoteControlCommand(ISystemController systemController)
        {
            _systemController = systemController;
        }

        public abstract void Execute();

        public Type ArgumentType
        {
            get
            {
                return typeof(T);
            }
        }
    }
}
