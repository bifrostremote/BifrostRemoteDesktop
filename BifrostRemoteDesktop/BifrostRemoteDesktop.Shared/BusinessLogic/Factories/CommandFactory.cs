using BifrostRemoteDesktop.BusinessLogic.Models.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Factories
{
    public static class CommandFactory
    {
        public enum CommandType
        {
            MovePointer,
            UpdatePointerState
        }

        public static ICommand CreateCommand(CommandType commandType, string[] commandArgs, ISystemController _systemController)
        {
            switch (commandType)
            {
                case CommandType.MovePointer:
                    {
                        return CreateMovePointerCommand(commandArgs, _systemController);
                    }
                default:
                    {
                        throw new ArgumentException($"The argument of parameter {nameof(commandType)} does not corrospond to a any known command.");
                    }
            }
            throw new ArgumentException($"The argument of parameter {nameof(commandType)} does not corrospond to a any known command.");
        }

        public static MovePointerCommand CreateMovePointerCommand(string[] commandArgs, ISystemController systemController)
        {
            ArgumentException nonsufficientArgumentsException = new ArgumentException($"The length of {nameof(commandArgs)} is not adequate for creating a ${nameof(MovePointerCommand)}. Two doubles needs to be provided.");
            if (commandArgs.Length == 2)
            {
                double x;
                double y;
                if (double.TryParse(commandArgs[0], out x)
                    && double.TryParse(commandArgs[1], out y))
                {
                    return new MovePointerCommand(systemController, x, y);
                }
                else
                {
                    throw nonsufficientArgumentsException;
                }
            }
            else
            {
                throw nonsufficientArgumentsException;
            }
        }
    }
}
