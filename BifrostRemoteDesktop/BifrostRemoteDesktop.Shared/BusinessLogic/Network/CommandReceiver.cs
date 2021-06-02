using BifrostRemote.Network;
using BifrostRemoteDesktop.BusinessLogic.Enums;
using BifrostRemoteDesktop.BusinessLogic.Factories;
using BifrostRemoteDesktop.BusinessLogic.Models;
using BifrostRemoteDesktop.BusinessLogic.Models.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BifrostRemoteDesktop.BusinessLogic.Network
{

    public class CommandReceiver
    {
        private ISystemController _systemController;
        private Thread thread;


        public CommandReceiver(ISystemController systemController)
        {
            _systemController = systemController;
        }

        public void Start()
        {
            if (!thread.IsAlive)
            {
                thread = new Thread(Listen);
                thread.Start(this);
            }
        }

        private void Stop()
        {
            if (thread.IsAlive)
            {
                thread.Join();
            }
        }

        private static void Listen(object obj)
        {
            if (obj is CommandReceiver commandReceiver)
            {
                IPAddress localhost = IPAddress.Parse("127.0.0.1");
                TcpListener listener = new TcpListener(localhost, TransmissionContext.INPUT_TCP_PORT);

                listener.Start();

                byte[] buffer = new byte[TransmissionContext.RECEIVER_BUFFER_SIZE];
                string data = string.Empty;
                while (true)
                {
                    TcpClient receiver = listener.AcceptTcpClient();
                    NetworkStream stream = receiver.GetStream();
                    int i;

                    while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        data += Encoding.UTF8.GetString(buffer, 0, i);
                        if (TryFindAndRemoveNextPackage(ref data, out string package))
                        {

                        }

                        ICommand command = ParseCommandFromPackage(commandReceiver, package);
                        command.Execute();
                    }

                    receiver.Close();
                }
            }
        }

        /// <summary>
        /// This method will try to find the next package in a data string.
        /// If a package is found it will remove it from the data string and return true.
        /// </summary>
        /// <param name="data">The data string to attempt package extraction on.</param>
        /// <param name="package">The found package. (string.Empty if no package is found.)</param>
        /// <returns>Returns true if a package was found.</returns>
        private static bool TryFindAndRemoveNextPackage(ref string data, out string package)
        {
            int startCharIndex = data.IndexOf(TransmissionContext.START_OF_TEXT_CHAR) + 1;
            int endCharIndex = data.IndexOf(TransmissionContext.END_OF_TEXT_CHAR, startCharIndex) - 1;

            //Check data for complete package.
            if (startCharIndex == -1 || endCharIndex == -1)
            {
                package = string.Empty;
                return false;
            }

            int packageSize = endCharIndex - startCharIndex;
            package = data.Substring(startCharIndex, packageSize);
            data = data.Remove(startCharIndex - 1, packageSize + 1);
            return true;
        }

        private static ICommand ParseCommandFromPackage(CommandReceiver commandReceiver, string package)
        {
            string[] parts = package.Split(TransmissionContext.TEXT_SEGMENTATION_CHAR);

            if(!Enum.TryParse(parts[0], out CommandType commandType))
            {
                throw new InvalidCastException();
            }

            RemoteControlCommandArgs commandArgs = JsonConvert.DeserializeObject<RemoteControlCommandArgs>(parts[1]);
            ICommand command = CommandFactory.CreateCommand(commandType, commandArgs, commandReceiver._systemController);
            return command;
        }
    }
}